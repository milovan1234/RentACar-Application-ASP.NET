using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class DateStartResValidation : ValidationAttribute
    {
        private ModelContext _context;
        public DateStartResValidation()
        {
            _context = new ModelContext();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reservation = (Reservation)validationContext.ObjectInstance;
            if (reservation.BeginDate.Date < DateTime.Now.Date)
            {
                return new ValidationResult("Date must be greater than or equal to today!");
            }            
            return ValidationResult.Success;
        }
    }
}