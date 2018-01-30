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
        // httpclient to be used by all public methods
        HttpClient client = new HttpClient();
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

        public async Task<string> Execute()
        {
            var user = Environment.GetEnvironmentVariable("CartegraphLogin");
            var pass = Environment.GetEnvironmentVariable("CartegraphPass");
            var sharepointUrl = "https://cgweb06.cartegraphoms.com/PittsburghPA/api/v1/classes/cgLaborClass?fields=OID,InactiveField,FullNameField,DepartmentField,HomePhoneField,CellularField";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue ( "Basic", 
                Convert.ToBase64String(
                System.Text.ASCIIEncoding.ASCII.GetBytes(
                string.Format("{0}:{1}", user, pass))));
            string content = await client.GetStringAsync(sharepointUrl);
            return content;
        }

        // Post callout data async from ajax call
    
        public void PostCallout(Contact model)
        {
            //await Execute(model);
        }
        
        // public async Task Execute(Contact model)
        // {
        //     var sharepointUrl = "https://cityofpittsburgh.sharepoint.com/sites/PublicSafety/ACC/_api/web/lists/GetByTitle('Animals')/items";
        //     client.DefaultRequestHeaders.Clear();
        //     client.DefaultRequestHeaders.Authorization = 
        //         new AuthenticationHeaderValue ("Bearer", SessionToken);
        //     client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
        //     client.DefaultRequestHeaders.Add("X-RequestDigest", "form digest value");
        //     client.DefaultRequestHeaders.Add("X-HTTP-Method", "POST");

        //     var json = 
        //         String.Format
        //         ("{{'__metadata': {{ 'type': 'SP.Data.AnimalsItem' }}, 'Type' : '{0}', 'Breed' : '{1}', 'Coat' : '{2}', 'Sex' : '{3}', 'LicenseNumber' : '{4}', 'RabbiesVacNo' : '{5}', 'RabbiesVacExp' : '{6}', 'Veterinarian' : '{7}', 'LicenseYear' : '{8}', 'Age' : '{9}', 'AddressID' : '{10}', 'AdvisoryID' : '{11}', 'Name' : '{12}' }}",
        //             model.OID, // 0
        //             model.Called, // 1
        //             model.Accepted, //2
        //             model.FullName, // 3
        //             model.Department); // 12
                
        //     client.DefaultRequestHeaders.Add("ContentLength", json.Length.ToString());
        //     try
        //     {
        //         StringContent strContent = new StringContent(json);               
        //         strContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");
        //         HttpResponseMessage response = client.PostAsync(sharepointUrl, strContent).Result;
                        
        //         response.EnsureSuccessStatusCode();
        //         var content = await response.Content.ReadAsStringAsync();
        //     }
        //     catch (Exception ex)
        //     {
        //         System.Diagnostics.Debug.WriteLine(ex.Message);
        //     }
        // }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

