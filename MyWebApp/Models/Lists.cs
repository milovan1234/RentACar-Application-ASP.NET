using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Lists
    {
        public static List<Reservation> CurrentlyRes { get; set; }
        public static List<User> Users { get; set; }
        public static List<UserMessage> UserMessages { get; set; }
    }
}