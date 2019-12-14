using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.ViewModels
{
    public class ShowReservationViewModel
    {
        public Automobile Automobile { get; set; }
        public IEnumerable<Offer> Offers { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}