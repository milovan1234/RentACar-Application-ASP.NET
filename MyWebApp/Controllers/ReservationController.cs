using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MyWebApp.ViewModels;

namespace MyWebApp.Controllers
{
    public class ReservationController : Controller
    {
        private ModelContext _context;
        public ReservationController()
        {
            _context = new ModelContext();
            if (Lists.CurrentlyRes == null)
                Lists.CurrentlyRes = new List<Reservation>();
        }
        public ActionResult ReservationPage(int id)
        {
            if (Session["Id"] != null && (Session["UserRank"].ToString() == "Customer" || Session["UserRank"].ToString() == "Admin"))
            {
                var autoInDb = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift).SingleOrDefault(a => a.Id == id);
                if (autoInDb != null)
                {
                    var viewModel = new ShowReservationViewModel
                    {
                        Automobile = autoInDb,
                        Offers = _context.Offers.Where(o => o.Automobile.Id == autoInDb.Id).ToList(),
                        Reservations = _context.Reservations.Where(r => r.Automobile.Id == autoInDb.Id).ToList()
                    };
                    return View("ReservationForm", viewModel);
                }
                else
                    return HttpNotFound();
            }
            else
            {
                Session["ResPageId"] = id;
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult NewReservation(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                int userId = Convert.ToInt32(Session["Id"]);
                var viewModel = new NewReservationViewModel
                {
                    Reservation = new Reservation
                    {
                        Automobile = _context.Automobiles
                        .Include(a => a.Gearshift)
                        .Include(a => a.CarBody)
                        .Include(a => a.NumberOfDoor).Single(a => a.Id == id),
                        User = _context.Users.Single(u => u.Id == userId)
                    },
                    Offers = _context.Offers.Where(o => o.AutomobileId == id).ToList(),
                    Reservations = _context.Reservations.Where(r => r.AutomobileId == id).ToList()
                };
                return View("NewReservation", viewModel);
            }
            else
                return HttpNotFound();
        }
        public ActionResult Save(Reservation reservation)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                if (!ModelState.IsValid)
                {
                    reservation.Automobile = _context.Automobiles.Include(a => a.Gearshift)
                        .Include(a => a.CarBody)
                        .Include(a => a.NumberOfDoor).Single(a => a.Id == reservation.AutomobileId);
                    reservation.User = _context.Users.Single(u => u.Id == reservation.UserId);
                    var viewModel = new NewReservationViewModel
                    {
                        Reservation = reservation,
                        Offers = _context.Offers.Where(o => o.AutomobileId == reservation.AutomobileId).ToList()
                    };
                    int userId = Convert.ToInt32(Session["Id"]);
                    if (_context.Reservations.Count() > 0)
                        viewModel.Reservations = _context.Reservations.Where(r => r.AutomobileId == reservation.AutomobileId).Include(r => r.Automobile);
                    else
                        viewModel.Reservations = new List<Reservation>();
                    return View("NewReservation", viewModel);
                }
                else
                {
                    double total = 0;
                    foreach (var offer in _context.Offers)
                    {
                        if (offer.BeginDate.Date <= reservation.BeginDate.Date &&
                            offer.DateStop.Date >= reservation.DateStop.Date && offer.AutomobileId == reservation.AutomobileId)
                        {
                            total = (reservation.DateStop - reservation.BeginDate).TotalDays * offer.PriceAtDay;
                        }
                    }
                    int countChart = Convert.ToInt32(Session["CountChart"]);
                    ++countChart;
                    Session["CountChart"] = countChart.ToString();
                    reservation.TotalPrice = total;
                    reservation.Automobile = _context.Automobiles.Single(a => a.Id == reservation.AutomobileId);
                    Lists.CurrentlyRes.Add(reservation);
                    int userId = Convert.ToInt32(Session["Id"]);
                    List<Reservation> example = new List<Reservation>();
                    var model = new NewReservationViewModel
                    {
                        Reservations = Lists.CurrentlyRes.Where(r => r.UserId == reservation.UserId)
                    };
                    if (_context.Reservations.Count() > 0)
                        model.YourReservations = _context.Reservations.Where(r => r.UserId == userId).Include(r => r.Automobile);
                    else
                        model.YourReservations = new List<Reservation>();
                    Session["NewReservation"] = "true";

                    return RedirectToAction("YourReservations", model);
                }
            }
            return HttpNotFound();
        }
        public ActionResult YourReservations()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                int userId = Convert.ToInt32(Session["Id"]);
                var viewModel = new NewReservationViewModel
                {
                    Reservations = Lists.CurrentlyRes.Where(r => r.UserId == userId)
                };
                if (_context.Reservations.Count() > 0)
                    viewModel.YourReservations = _context.Reservations.Where(r => r.UserId == userId).Include(r => r.Automobile);
                else
                    viewModel.YourReservations = new List<Reservation>();
                return View(viewModel);
            }
            return HttpNotFound();
        }
        public ActionResult DeleteResCheck(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                int index = id;
                if (index < Lists.CurrentlyRes.Count())
                {
                    Lists.CurrentlyRes.RemoveAt(index);
                    int userId = Convert.ToInt32(Session["Id"]);
                    var viewModel = new NewReservationViewModel
                    {
                        Reservations = Lists.CurrentlyRes.Where(r => r.UserId == userId)
                    };
                    if (_context.Reservations.Count() > 0)
                        viewModel.YourReservations = _context.Reservations.Where(r => r.UserId == userId).Include(r => r.Automobile);
                    else
                        viewModel.YourReservations = new List<Reservation>();
                    int countChart = Convert.ToInt32(Session["CountChart"]);
                    --countChart;
                    Session["CountChart"] = countChart.ToString();
                    return View("YourReservations", viewModel);
                }
                return HttpNotFound();
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult BuyReservation()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer" && Lists.CurrentlyRes.Count() > 0)
            {
                for (int i = 0; i < Lists.CurrentlyRes.Count(); i++)
                {
                    Lists.CurrentlyRes[i].Automobile = null;
                    _context.Reservations.Add(Lists.CurrentlyRes[i]);
                }
                int countChart = Convert.ToInt32(Session["CountChart"]);
                countChart -= Lists.CurrentlyRes.Count();
                Session["CountChart"] = countChart.ToString();
                Lists.CurrentlyRes.Clear();
                int userId = Convert.ToInt32(Session["Id"]);
                _context.SaveChanges();
                var viewModel = new NewReservationViewModel
                {
                    YourReservations = _context.Reservations.Where(r => r.UserId == userId).Include(r => r.Automobile),
                    Reservations = new List<Reservation>()
                };
                return View("YourReservations", viewModel);
            }
            return HttpNotFound();
        }
        public ActionResult ValidReservation()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                int userId = Convert.ToInt32(Session["Id"]);
                var viewModel = new NewReservationViewModel
                {
                    YourReservations = _context.Reservations.Where(r => r.UserId == userId)
                                                            .Include(r => r.User)
                                                            .Include(r => r.Automobile).ToList()
                };
                return View(viewModel);
            }
            return HttpNotFound();
        }
        [HttpDelete]
        public ActionResult DeleteYourRes(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Customer")
            {
                var reservationInDb = _context.Reservations.Single(r => r.Id == id);
                int userId = Convert.ToInt32(Session["Id"]);
                if (reservationInDb != null)
                {
                    _context.Reservations.Remove(reservationInDb);
                    _context.SaveChanges();
                    var viewModel = new NewReservationViewModel
                    {
                        YourReservations = _context.Reservations.Where(r => r.UserId == userId).Include(r => r.Automobile)
                        .Include(r => r.User).ToList()
                    };
                    return View("ValidReservation", viewModel);
                }
                return HttpNotFound();
            }
            return HttpNotFound();
        }
    }
}