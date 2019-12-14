using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.ViewModels
{
    public class NewAutomobileViewModel
    {
        public Automobile Automobile { get; set; }
        public IEnumerable<NumberOfDoor> NumberOfDoors { get; set; }
        public IEnumerable<CarBody> CarBodies { get; set; }
        public IEnumerable<Gearshift> Gearshifts { get; set; }
    }
}