using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace samadApp.Models.DataTransportObject
{
    public class SellDto
    {
        [Required]
        public string FoodName { get; set; }
       [Required]
        public string College { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}