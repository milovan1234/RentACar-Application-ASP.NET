using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class DateStartValidation : ValidationAttribute
    {
        private ModelContext _context;
        public DateStartValidation()
        {
            _context = new ModelContext();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var offer = (Offer)validationContext.ObjectInstance;
            bool check = true;
            foreach(Offer o in _context.Offers)
            {
                if (offer.BeginDate.Date >= o.BeginDate.Date && offer.BeginDate.Date <= o.DateStop.Date && offer.AutomobileId == o.AutomobileId)
                {
                    check = false;
                }
            }
            if (offer.BeginDate.Date < DateTime.Now.Date)
            {
                return new ValidationResult("Date must be greater than or equal to today!");
            }
            else if (!check)
            {
                return new ValidationResult("There is an offer for the date entered and cannot be duplicate!");
            }
            else
                return ValidationResult.Success;
        }
    }
}