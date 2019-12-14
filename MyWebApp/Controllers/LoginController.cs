using MyWebApp.Models;
using MyWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace MyWebApp.Controllers
{
    public class LoginController : Controller
    {
        private ModelContext _context;
        public LoginController()
        {
            _context = new ModelContext();
            if (Lists.Users == null)
                Lists.Users = new List<User>();
        }
        public ActionResult Index()
        {
            if (Session["Id"] == null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        public ActionResult Login(User user)
        {
            var userInDb = _context.Users.FirstOrDefault(c => c.Username == user.Username);
            if (userInDb == null)
            {
                Session["ErrorMessage"] = "Account not existing. Go to registration.";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var pass = EncDecPassword.DecryptPassword(userInDb.Password);
                userInDb = user.Password == pass ? userInDb : null;
                if (userInDb == null)
                {
                    Session["ErrorMessage"] = "The password is incorrect. Check the 'caps lock'.";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    Session["Id"] = userInDb.Id;
                    Session["ImagePath"] = userInDb.ImagePath;
                    Session["FirstName"] = userInDb.FirstName;
                    Session["UserRank"] = userInDb.UserRank;
                    Session["Username"] = userInDb.Username;
                    Session["ErrorMessage"] = null;
                    Lists.Users.Add(userInDb);
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        public ActionResult Logout()
        {
            int userId = Convert.ToInt32(Session["Id"]);
            var userInDb = _context.Users.Single(u => u.Id == userId);
            for (int i = 0; i < Lists.Users.Count(); i++)
            {
                if (Lists.Users[i].Id == userInDb.Id)
                {
                    Lists.Users.RemoveAt(i);
                    break;
                }
            }
            Session.Abandon();
            if(Lists.CurrentlyRes != null)
                Lists.CurrentlyRes.Clear();
            return RedirectToAction("Index", "Home");
        }        
    }
}