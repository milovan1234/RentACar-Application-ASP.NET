using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class UserMessage
    {
        public int Id { get; set; }
        public User UserFrom { get; set; }
        public int UserFromId { get; set; }

        public User UserTo { get; set; }
        public int UserToId { get; set; }

        public string Message { get; set; }
        public DateTime DateTimeSend { get; set; }

        public bool ReadMessage { get; set; }
    }
}