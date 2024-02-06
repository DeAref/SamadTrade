using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace samadApp.Models
{
    public class Price
    {
        [Key]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }
        public int FoodPrice { get; set; }
    }
}