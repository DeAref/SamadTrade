using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;

namespace samadApp.Models.DataTransportObject
{
    public class BuyDto
    {
        [Required]
        public string? DateOfFood { get; set; }
        [Required]
        public string? NameOfFood { get; set; }
    }
}