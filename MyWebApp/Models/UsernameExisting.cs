using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class UsernameExisting : ValidationAttribute
    {
        private ModelContext _context;
        public UsernameExisting()
        {
            _context = new ModelContext();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (User)validationContext.ObjectInstance;
            var userInDb = new User();
            if (user.Id == 0)
                userInDb = _context.Users.FirstOrDefault(c => c.Username == user.Username);
            else
                userInDb = _context.Users.Where(c => c.Username == user.Username && c.Id != user.Id).FirstOrDefault();

            if (userInDb == null)
                return ValidationResult.Success;
            else
                return new ValidationResult("The Customer already exists.");
        }
    }
}