using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace samadApp.Models
{
    public class Role:IdentityRole
    {
        public string? Description{get;set;}

    }
}