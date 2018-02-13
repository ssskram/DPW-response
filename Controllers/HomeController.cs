using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DPW_response.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;

namespace DPW_response.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // inject dependency on usermanager
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        // httpclient to be used by all public methods
        HttpClient client = new HttpClient();
        public async Task<IActionResult>  Index()
        {
            await ExecuteGet();
            var content = ExecuteGet().Result; 
            dynamic jo = JObject.Parse(content)["cgLaborClass"];
            List<Contact> Humans = new List<Contact>();
            var dateformat = "MM/dd/yyyy";
                foreach (var item in jo)
                {
                    if (item.InactiveField == false && 
                        item.FullNameField != "HE Mechanic" &&
                        item.DepartmentField != null &&
                        item.DepartmentField != "DOMI - Design" &&
                        item.DepartmentField != "DOMI - Traffic Control" &&
                        item.DepartmentField != "Administration" && 
                        item.DepartmentField != "Asphalt" && 
                        item.DepartmentField != "DOMI - Signs and Markings" &&
                        item.DepartmentField != "Environmental Services" &&
                        item.CityRateNameField != "Clerk") {
                        Contact contact1 = new Contact() {
                            OID = item.Oid,
                            FullName = item.FullNameField,
                            Department = item.DepartmentField,
                            HomePhone = item.HomePhoneField,
                            CellPhone = item.CellularField,
                            HireDate = item.HireDateField.ToString(dateformat),
                            ReleaseDate = item.ReleaseDateField.ToString(dateformat),
                            BargainingUnit = item.BargainingUnitSeniorityDateField.ToString(dateformat),
                            SubUnionSeniorityDate = item.SubUnionSeniorityDateField.ToString(dateformat),
                            SubUnion = item.SubUnionField
                        };
                        Humans.Add(contact1);  
                    }
                }
            return View(Humans);
        }

        public async Task<string> ExecuteGet()
        {
            var user = Environment.GetEnvironmentVariable("CartegraphLogin");
            var pass = Environment.GetEnvironmentVariable("CartegraphPass");
            var cartegraphUrl = "https://cgweb06.cartegraphoms.com/PittsburghPA/api/v1/classes/cgLaborClass?fields=OID,InactiveField,FullNameField,DepartmentField,HomePhoneField,CellularField,HireDateField,ReleaseDateField,BargainingUnitSeniorityDateField,SubUnionSeniorityDateField,SubUnionField,CityRateNameField";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue ( "Basic", 
                Convert.ToBase64String(
                System.Text.ASCIIEncoding.ASCII.GetBytes(
                string.Format("{0}:{1}", user, pass))));
            string content = await client.GetStringAsync(cartegraphUrl);
            return content;
        }

        // Post callout data async from ajax call
        public async Task<IActionResult> PostCallout(Contact model)
        {
            await ExecutePost(model);
            return RedirectToAction(nameof(HomeController.Index));
        }
        
        public async Task ExecutePost(Contact model)
        {
            var user = Environment.GetEnvironmentVariable("CartegraphLogin");
            var pass = Environment.GetEnvironmentVariable("CartegraphPass");
            var calledby = _userManager.GetUserName(HttpContext.User);
            var cartegraphUrl = "https://cgweb06.cartegraphoms.com/PittsburghPA/api/v1/Classes/cgLabor_OvertimeLogsClass";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-HTTP-Method", "POST");
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue ( "Basic", 
                Convert.ToBase64String(
                System.Text.ASCIIEncoding.ASCII.GetBytes(
                string.Format("{0}:{1}", user, pass))));
            var json =
                String.Format
                ("{{ 'cgLabor_OvertimeLogsClass' : [ {{ 'ParentOid' : '{0}' , 'CalledByField' : '{1}' , 'AcceptedField' : '{2}' }} ] }}",
                    model.OID, // 0
                    calledby, // 1
                    model.Accepted); //2
            client.DefaultRequestHeaders.Add("ContentLength", json.Length.ToString());
            try
            {
                StringContent strContent = new StringContent(json);               
                strContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");
                HttpResponseMessage response = client.PostAsync(cartegraphUrl, strContent).Result;
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

