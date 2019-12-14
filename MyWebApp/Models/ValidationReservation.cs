using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class ValidationReservation : ValidationAttribute
    {
        private ModelContext _context;
        public ValidationReservation()
        {
            _context = new ModelContext();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reservation = (Reservation)validationContext.ObjectInstance;
            List<Offer> offers = _context.Offers.Where(o => o.Automobile.Id == reservation.AutomobileId).ToList();
            List<Reservation> reservations = _context.Reservations.Where(r => r.Automobile.Id == reservation.AutomobileId).ToList();
            bool check = false;
            bool checkR = false;
            bool checkChart = false;
            for (int i = 0; i < offers.Count(); i++)
            {
                if (reservation.BeginDate.Date >= offers[i].BeginDate.Date && reservation.BeginDate.Date <= offers[i].DateStop.Date &&
                    reservation.DateStop.Date >= offers[i].BeginDate.Date && reservation.DateStop.Date <= offers[i].DateStop.Date)
                    check = true;
            }
            for (int i = 0; i < reservations.Count(); i++)
            {
                if ((reservation.BeginDate.Date >= reservations[i].BeginDate.Date && reservation.BeginDate.Date <= reservations[i].DateStop.Date) ||
                    (reservation.DateStop.Date >= reservations[i].BeginDate.Date && reservation.DateStop.Date <= reservations[i].DateStop.Date))
                    checkR = true;
            }
            if (Lists.CurrentlyRes.Count() > 0)
            {
                for (int i = 0; i < Lists.CurrentlyRes.Count(); i++)
                {
                    if ((reservation.BeginDate.Date >= Lists.CurrentlyRes[i].BeginDate.Date && reservation.BeginDate.Date <= Lists.CurrentlyRes[i].DateStop.Date) ||
                        (reservation.DateStop.Date >= Lists.CurrentlyRes[i].BeginDate.Date && reservation.DateStop.Date <= Lists.CurrentlyRes[i].DateStop.Date))
                        checkChart = true;
                }
            }
            if (reservations.Count() == 0)
                checkR = false;
            if (!check)
                return new ValidationResult("The reservation is not valid!");
            else if (checkR)
                return new ValidationResult("Reservation for these Dates is exist!");
            else if (checkChart)
                return new ValidationResult("Reservation for these Dates is exist on your bill!");
            else
                return ValidationResult.Success;
        }
    }
}