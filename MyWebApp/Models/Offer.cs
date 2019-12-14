using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Offer
    {
        public int Id { get; set; }
        [Required]
        [DateStartValidation]
        public DateTime BeginDate { get; set; }
        [Required]
        [DateStopValidation]
        public DateTime DateStop { get; set; }
        [Required]
        public double PriceAtDay { get; set; }
        public Automobile Automobile { get; set; }
        public int AutomobileId { get; set; }
    }
}