using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class DateStopValidation : ValidationAttribute
    {
        private ModelContext _context;
        public DateStopValidation()
        {
            _context = new ModelContext();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var offer = (Offer)validationContext.ObjectInstance;
            bool check = true;
            foreach (Offer o in _context.Offers)
            {
                if (offer.DateStop.Date >= o.BeginDate.Date && offer.DateStop.Date <= o.DateStop.Date && offer.AutomobileId == o.AutomobileId)
                {
                    check = false;
                }
            }
            if (offer.DateStop.Date <= offer.BeginDate.Date && offer.BeginDate != null)
                return new ValidationResult("DateStop is invalid!");
            else if (!check)
            {
                return new ValidationResult("There is an offer for the date entered and cannot be duplicate!");
            }
            else
                return ValidationResult.Success;
        }
    }
}