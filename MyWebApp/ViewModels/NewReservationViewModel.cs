using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.ViewModels
{
    public class NewReservationViewModel
    {
        public Reservation Reservation { get; set; }
        public IEnumerable<Offer> Offers { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
        public IEnumerable<Reservation> YourReservations { get; set; }
        public User User { get; set; }
    }
}