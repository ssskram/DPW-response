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

namespace DPW_response.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<IActionResult>  Index()
        {
            await Execute();
            var content = Execute().Result; 
            dynamic jo = JObject.Parse(content)["cgLaborClass"];
            List<Contact> Humans = new List<Contact>();
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
                        item.DepartmentField != "Environmental Services") {
                        Contact contact1 = new Contact() {
                            OID = item.Oid,
                            FullName = item.FullNameField,
                            Department = item.DepartmentField,
                            HomePhone = item.HomePhoneField,
                            CellPhone = item.CellularField
                        };
                        Humans.Add(contact1);  
                    }
                }
            return View(Humans);
        }

        static async Task<string> Execute()
        {

            var user = Environment.GetEnvironmentVariable("CartegraphLogin");
            var pass = Environment.GetEnvironmentVariable("CartegraphPass");
            var sharepointUrl = "https://cgweb06.cartegraphoms.com/PittsburghPA/api/v1/classes/cgLaborClass?fields=OID,InactiveField,FullNameField,DepartmentField,HomePhoneField,CellularField";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue ( "Basic", 
                Convert.ToBase64String(
                System.Text.ASCIIEncoding.ASCII.GetBytes(
                string.Format("{0}:{1}", user, pass))));
            string content = await client.GetStringAsync(sharepointUrl);
            return content;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}