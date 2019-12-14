using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public Automobile Automobile { get; set; }
        public int AutomobileId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Begin Date Reservation")]
        [DateStartResValidation]
        [ValidationReservation]
        public DateTime BeginDate { get; set; }
        [Required]
        [Display(Name = "Stop Date Reservation")]
        [DateStopResValidation]
        [ValidationReservation]
        public DateTime DateStop { get; set; }
        [Required]
        public double TotalPrice { get; set; }
    }
}