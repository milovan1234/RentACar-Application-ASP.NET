using MyWebApp.Models;
using MyWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class RegisterController : Controller
    {
        private ModelContext _context;
        public RegisterController()
        {
            _context = new ModelContext();
        }
        public ActionResult Index()
        {
            if (Session["Id"] == null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Save(User user, HttpPostedFileBase ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new NewUserViewModel
                {
                    User = user
                };
                return View("Index", viewModel);
            }
            if (user.Id == 0)
            {
                if (ImageFile != null)
                {
                    string filename = Path.GetFileName(ImageFile.FileName);
                    user.ImagePath = "/Images/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                    ImageFile.SaveAs(filename);
                }
                else
                {
                    user.ImagePath = "/Images/LOG-IN.png";
                }
                user.UserRank = "Customer";
                user.Password = EncDecPassword.EncryptPassword(user.Password);
                _context.Users.Add(user);
            }                
            _context.SaveChanges();
            return RedirectToAction("Index", "Login");
        }        
    }
}