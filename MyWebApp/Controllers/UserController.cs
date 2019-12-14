using MyWebApp.Models;
using MyWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MyWebApp.Controllers
{

    public class UserController : Controller
    {
        private ModelContext _context;
        public UserController()
        {
            _context = new ModelContext();
            if (Lists.UserMessages == null)
                Lists.UserMessages = new List<UserMessage>();
        }
        public ActionResult AllCustomers()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                int userId = Convert.ToInt32(Session["Id"]);
                var users = _context.Users.Where(u => u.UserRank == "Customer").ToList();
                var viewModel = new NewRandomViewModel
                {
                    Users = users
                };
                return View("Index", viewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult Save(User user, HttpPostedFileBase UserImage)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                if (!ModelState.IsValid)
                {
                    var viewModel = new NewRandomViewModel
                    {
                        User = user
                    };
                    return View("EditUser", viewModel);
                }
                if (user.Id == 0)
                {
                    if (UserImage != null)
                    {
                        string filename = Path.GetFileName(UserImage.FileName);
                        user.ImagePath = "/Images/" + filename;
                        filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                        UserImage.SaveAs(filename);
                    }
                    else
                    {
                        user.ImagePath = "/Images/LOG-IN.png";
                    }
                    user.UserRank = "Customer";
                    user.Password = EncDecPassword.EncryptPassword(user.Password);
                    _context.Users.Add(user);
                }
                else
                {
                    var userInDb = _context.Users.FirstOrDefault(u => u.Id == user.Id);
                    if (userInDb == null)
                        return HttpNotFound();
                    //ViewBag.UserIdForExisting = userInDb.Id;
                    userInDb.FirstName = user.FirstName;
                    userInDb.LastName = user.LastName;
                    userInDb.Username = user.Username;
                    userInDb.Password = EncDecPassword.EncryptPassword(user.Password);
                    if (UserImage != null)
                    {
                        string filename = Path.GetFileName(UserImage.FileName);
                        userInDb.ImagePath = "/Images/" + filename;
                        filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                        UserImage.SaveAs(filename);
                    }
                }
                _context.SaveChanges();
                var model = new NewRandomViewModel
                {
                    Users = _context.Users.Where(u => u.UserRank == "Customer").ToList()
                };
                return View("Index", model);
            }
            else
                return HttpNotFound();
        }
        public ActionResult EditUser(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Id == id);
                if (userInDb == null)
                    return HttpNotFound();
                userInDb.Password = EncDecPassword.DecryptPassword(userInDb.Password);
                var viewModel = new NewRandomViewModel
                {
                    User = userInDb
                };
                Session["UpdateUser"] = "True";
                return View("EditUser", viewModel);
            }
            else
                return HttpNotFound();
        }
        public ActionResult NewUser()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var viewModel = new NewRandomViewModel
                {
                    User = new User()
                };
                return View("EditUser", viewModel);
            }
            return HttpNotFound();
        }
        public ActionResult YourProfile()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                int userId = Convert.ToInt32(Session["Id"]);
                var userInDb = _context.Users.FirstOrDefault(u => u.Id == userId);
                userInDb.Password = EncDecPassword.DecryptPassword(userInDb.Password);
                var viewModel = new NewRandomViewModel
                {
                    User = userInDb
                };
                return View(viewModel);
            }
            return HttpNotFound();
        }
        public ActionResult SaveData(User user, HttpPostedFileBase UserImage)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                if (!ModelState.IsValid)
                {
                    var viewModel = new NewRandomViewModel
                    {
                        User = user
                    };
                    return View("YourProfile", viewModel);
                }
                var userInDb = _context.Users.FirstOrDefault(u => u.Id == user.Id);
                if (userInDb == null)
                    return HttpNotFound();
                userInDb.FirstName = user.FirstName;
                userInDb.LastName = user.LastName;
                userInDb.Username = user.Username;
                userInDb.Password = EncDecPassword.EncryptPassword(user.Password);
                if (UserImage != null)
                {
                    string filename = Path.GetFileName(UserImage.FileName);
                    userInDb.ImagePath = "/Images/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                    UserImage.SaveAs(filename);
                }
                _context.SaveChanges();
                Session["FirstName"] = userInDb.FirstName;
                Session["Username"] = userInDb.Username;
                Session["ImagePath"] = userInDb.ImagePath;
                Session["UpdateUser"] = "true";
                var model = new NewRandomViewModel
                {
                    User = _context.Users.FirstOrDefault(u => u.Id == user.Id)
                };
                return View("YourProfile", model);
            }
            return HttpNotFound();
        }
        [HttpPut]
        public ActionResult DeleteUserPhoto(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Id == id);
                if (userInDb == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    userInDb.ImagePath = "/Images/LOG-IN.png";
                    Session["ImagePath"] = "/Images/LOG-IN.png";
                    _context.SaveChanges();
                    var viewModel = new NewRandomViewModel
                    {
                        User = userInDb
                    };
                    return View("YourProfile", viewModel);
                }
            }
            return HttpNotFound();
        }
        public ActionResult UsersReservation(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Id == id);
                if (userInDb != null)
                {
                    var viewModel = new NewReservationViewModel
                    {
                        Reservations = _context.Reservations.Include(r => r.Automobile).Where(r => r.UserId == id).ToList(),
                        User = userInDb
                    };
                    return View(viewModel);
                }
                else
                    return HttpNotFound();
            }
            return HttpNotFound();
        }
        [HttpDelete]
        public ActionResult DeleteYourRes(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var reservationInDb = _context.Reservations.Single(r => r.Id == id);
                int userId = reservationInDb.UserId;
                if (reservationInDb != null)
                {
                    _context.Reservations.Remove(reservationInDb);
                    _context.SaveChanges();
                    var viewModel = new NewReservationViewModel
                    {
                        Reservations = _context.Reservations.Where(r => r.UserId == userId).Include(r => r.Automobile)
                        .Include(r => r.User).ToList(),
                        User = _context.Users.FirstOrDefault(u => u.Id == userId)
                    };
                    return View("UsersReservation", viewModel);
                }
                return HttpNotFound();
            }
            return HttpNotFound();
        }
        public ActionResult AdminComunications()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var viewModel = new NewComunicationsViewModel
                {
                    Users = _context.Users.Where(u => u.UserRank == "Customer"),
                    ActiveUsers = Lists.Users.Where(u => u.UserRank == "Customer")
                };
                return View("Comunications", viewModel);
            }
            return HttpNotFound();
        }
        public ActionResult UserChat(int id)
        {
            int userId = Convert.ToInt32(Session["Id"]);
            var userFrom = _context.Users.Single(u => u.Id == userId);
            var userTo = _context.Users.Single(u => u.Id == id);
            List<UserMessage> listMessages = new List<UserMessage>();
            if (userFrom.UserRank == "Admin" && userTo.UserRank == "Customer")
            {
                string fileName = userFrom.Username + userTo.Username + ".json";
                WriteReadJsonFile.ReadList<UserMessage>(ref listMessages, fileName);
                if (listMessages != null)
                {
                    foreach (var m in listMessages)
                    {
                        m.UserFrom = _context.Users.Single(u => u.Id == m.UserFromId);
                        m.UserTo = _context.Users.Single(u => u.Id == m.UserToId);
                        if (m.UserToId == userId)
                            m.ReadMessage = true;
                    }
                }
                WriteReadJsonFile.WriteList(listMessages, fileName);
                var viewModel = new NewChatViewModel
                {
                    UserMessage = new UserMessage
                    {
                        UserTo = userTo,
                        UserFrom = userFrom,
                        UserToId = userTo.Id,
                        UserFromId = userFrom.Id
                    },
                    UserMessages = listMessages,
                    Filename = fileName
                };
                return View(viewModel);
            }
            else if (userFrom.UserRank == "Customer" && userTo.UserRank == "Admin")
            {
                string fileName = userTo.Username + userFrom.Username + ".json";
                WriteReadJsonFile.ReadList<UserMessage>(ref listMessages, fileName);
                if (listMessages != null)
                {
                    foreach (var m in listMessages)
                    {
                        m.UserFrom = _context.Users.Single(u => u.Id == m.UserFromId);
                        m.UserTo = _context.Users.Single(u => u.Id == m.UserToId);
                        if (m.UserToId == userId)
                            m.ReadMessage = true;
                    }
                }
                WriteReadJsonFile.WriteList(listMessages, fileName);
                var viewModel = new NewChatViewModel
                {
                    UserMessage = new UserMessage
                    {
                        UserTo = userTo,
                        UserFrom = userFrom,
                        UserToId = userTo.Id,
                        UserFromId = userFrom.Id
                    },
                    UserMessages = listMessages,
                    Filename = fileName
                };
                return View(viewModel);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult SendMessage(UserMessage userMessage)
        {
            List<UserMessage> listMessages = new List<UserMessage>();
            userMessage.DateTimeSend = DateTime.Now;
            userMessage.ReadMessage = false;
            var userFrom = _context.Users.Single(u => u.Id == userMessage.UserFromId);
            var userTo = _context.Users.Single(u => u.Id == userMessage.UserToId);
            string fileName = "";
            if (userFrom.UserRank == "Admin" && userTo.UserRank == "Customer")
            {
                fileName = userFrom.Username + userTo.Username + ".json";
            }
            else if (userFrom.UserRank == "Customer" && userTo.UserRank == "Admin")
            {
                fileName = userTo.Username + userFrom.Username + ".json";
            }
            WriteReadJsonFile.ReadList<UserMessage>(ref listMessages, fileName);
            if (listMessages == null)
                listMessages = new List<UserMessage>();
            listMessages.Add(userMessage);
            WriteReadJsonFile.WriteList<UserMessage>(listMessages, fileName);
            if (listMessages != null)
            {
                foreach (var m in listMessages)
                {
                    m.UserFrom = _context.Users.Single(u => u.Id == m.UserFromId);
                    m.UserTo = _context.Users.Single(u => u.Id == m.UserToId);
                }
            }
            var viewModel = new NewChatViewModel
            {
                UserMessage = new UserMessage
                {
                    UserFrom = userFrom,
                    UserTo = userTo,
                    UserToId = userTo.Id,
                    UserFromId = userFrom.Id
                },
                UserMessages = listMessages,
                Filename = fileName
            };
            return View("UserChat", viewModel);
        }
        public ActionResult CustomerComunications()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                var viewModel = new NewComunicationsViewModel
                {
                    Users = _context.Users.Where(u => u.UserRank == "Admin"),
                    ActiveUsers = Lists.Users.Where(u => u.UserRank == "Admin")
                };
                return View("Comunications", viewModel);
            }
            return HttpNotFound();
        }
        public JsonResult GetMessages(string fileName)
        {
            List<UserMessage> listMessages = new List<UserMessage>();
            WriteReadJsonFile.ReadList(ref listMessages, fileName);
            if (listMessages != null)
            {
                foreach (var m in listMessages)
                {
                    m.UserFrom = _context.Users.Single(u => u.Id == m.UserFromId);
                    m.UserTo = _context.Users.Single(u => u.Id == m.UserToId);
                }
            }
            return Json(listMessages, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteChat(string fileName)
        {
            List<UserMessage> listMessages = new List<UserMessage>();
            WriteReadJsonFile.WriteList(listMessages, fileName);
            return Json(listMessages, JsonRequestBehavior.AllowGet);
        }
    }
}