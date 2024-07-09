using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Attendance_Application.Models
{
    public class RegisterDB
    {
        public int ID { get; set; }
        [DisplayName("User name")]
        [Required]
        public string UserName { get; set; }
        [DisplayName("Email Address")]
        [Required]
        public string Email { get; set; }
        [DisplayName("Password")]
        [Required]
        public string Password { get; set; }
        [DisplayName("Contact number")]
        [Required]
        public long PhNo { get; set; }
        [DisplayName("I agree to the")]
        [Required]
        public bool iAgree {  get; set; }
    }
}