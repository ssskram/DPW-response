using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DPW_response.Models
{
    public class Contact 
    {
        public string OID { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
    }
}