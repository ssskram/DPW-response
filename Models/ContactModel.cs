using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DPW_response.Models
{
    public class Contact
    {
        public string Called { get; set; }
        public string Accepted { get; set; }
        public string OID { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string HireDate { get; set; }
        public string ReleaseDate { get; set; }
        public string BargainingUnit { get; set; }
        public string SubUnionSeniorityDate { get; set; }
        public string SubUnion { get; set; }
        public string LotteryNumber { get; set; }
        public string CityRateName { get; set; }
    }
}