using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.ViewModels
{
    public class NewComunicationsViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<User> ActiveUsers { get; set; }
    }
}