using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Frist Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [UsernameExisting]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Min18Years]
        public DateTime Birthdate { get; set; }
        public string UserRank { get; set; }
        [Display(Name = "Your Image")]
        public string ImagePath { get; set; }
    }
}