using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class UserForCreationDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }

    public class UserResetPasswordForCreationDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class UserChangePasswordForCreationDTO
    {
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
    public class UserForUpdateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string HomeAddress { get; set; }
        public string Gender { get; set; }
        public string Phone  { get; set; }
    }


    public class UserVendorForCreationDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserForSetThreshold
    {
        public Guid MinistryId { get; set; }
        public double Threshold { get; set; }
    }
}

