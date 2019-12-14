using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.ViewModels
{
    public class ShowOffersAuto
    {
        public Automobile Automobile { get; set; }
        public IEnumerable<Offer> Offers { get; set; }
    }
}