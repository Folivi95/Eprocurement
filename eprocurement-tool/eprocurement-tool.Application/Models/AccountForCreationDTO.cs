using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EGPS.Application.Models
{
    public class AccountForCreationDTO
    {
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactEmail { get; set; }
        public string Website { get; set; }
        public string Password { get; set; }
    }

    public class AccountForUpdateDTO
    {
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string BusinessType { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
    }

}
