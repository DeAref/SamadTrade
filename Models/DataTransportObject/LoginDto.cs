using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;

namespace samadApp.Models.DataTransportObject
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string? UserName{get; set;}
        [Required]
        [DataType(DataType.Password)] 
        public string? Password { get; set; }
        [DefaultValue(false)]
        public bool RememmberMe { get; set; }
    }
}