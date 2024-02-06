using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace samadApp.Models
{
    public class Users:IdentityUser
    {
        public string? Fname{get;set;}
        public string? College{get;set;}
        public string? Lname{get;set;}
        [DefaultValue("0")]
        public string Wallet { get; set; }
    }
}