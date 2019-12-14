using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.ViewModels
{
    public class NewRandomViewModel
    {
        public Automobile Automobile { get; set; }
        public User User { get; set; }
        public Reservation Reservation { get; set; }
        public Offer Offer { get; set; }
        public IEnumerable<Automobile> Automobiles { get; set; }
        public IEnumerable<Automobile> SearchesAutos { get; set; }
        public IEnumerable<NumberOfDoor> NumberOfDoors { get; set; }
        public IEnumerable<CarBody> CarBodies { get; set; }
        public IEnumerable<Gearshift> Gearshifts { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Offer> Offers { get; set; }


        public int countPage { get; set; }
        public int NumberOfPage { get; set; }
        public string Brand { get; set; }
    }
}