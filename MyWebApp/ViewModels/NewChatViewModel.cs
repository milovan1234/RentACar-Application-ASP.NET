using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.ViewModels
{
    public class NewChatViewModel
    {
        public UserMessage UserMessage { get; set; }
        public IEnumerable<UserMessage> UserMessages { get; set; }
        public string Filename { get; set; }
        public int Newmessages { get; set; }
    }
}