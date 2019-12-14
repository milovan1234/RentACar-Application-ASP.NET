using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class DateStopResValidation : ValidationAttribute
    {
        private ModelContext _context;
        public DateStopResValidation()
        {
            _context = new ModelContext();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reservation = (Reservation)validationContext.ObjectInstance;
            if (reservation.BeginDate.Date >= reservation.DateStop.Date && reservation.BeginDate != null)
            {
                return new ValidationResult("DateStop is invalid!");
            }
            else
                return ValidationResult.Success;
        }
    }
}