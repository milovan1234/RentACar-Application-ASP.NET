using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class ModelContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Automobile> Automobiles { get; set; }
        public DbSet<NumberOfDoor> NumberOfDoors { get; set; }
        public DbSet<CarBody> CarBodies { get; set; }
        public DbSet<Gearshift> Gearshifts { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }        
    }
}