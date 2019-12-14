using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Min18Years : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (User)validationContext.ObjectInstance;
            int years = DateTime.Now.Year - user.Birthdate.Year;
            if (years >= 18)
                return ValidationResult.Success;
            else
                return new ValidationResult("The buyer must be of legal age.");
        }
    }
}