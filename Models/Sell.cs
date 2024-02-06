using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace samadApp.Models
{
    public class Sell
    {
         [Key]
        public int Id { get; set; }
        [Required]
        public string EmployeeUserId { get; set; }
        
        public string? CustomerUserId { get; set; }
        [Required]
        public string FoodName { get; set; }
        [Required]
        public string QrCode { get; set; }
        [DeniedValues(false)]
        public bool? IsSold { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string College { get; set; }
    }
}