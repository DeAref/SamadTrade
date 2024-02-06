using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace samadApp.Models.DataTransportObject
{
    public class Register
    {
        [Required]
        public string Fname { get; set; }
        public string College { get; set; }
        public string Lname { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }  
        [DataType(DataType.Password)]
        [Required] 
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required] 
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}