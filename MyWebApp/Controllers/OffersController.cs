using MyWebApp.Models;
using MyWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MyWebApp.Controllers
{
    public class OffersController : Controller
    {
        private ModelContext _context;
        public OffersController()
        {
            _context = new ModelContext();
        }        
        public ActionResult ShowOffers(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var automobile = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift).SingleOrDefault(a => a.Id == id);
                if (automobile == null)
                    return HttpNotFound();
                var offers = _context.Offers.Include(a => a.Automobile).Where(a => a.AutomobileId == automobile.Id).ToList();
                var viewModel = new ShowOffersAuto
                {
                    Automobile = automobile,
                    Offers = offers
                };
                return View("Index",viewModel);
            }
            else
                return HttpNotFound();
        }
        public ActionResult NewOffer(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var automobileInDb = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift)
                    .Single(a => a.Id == id);
                if (automobileInDb == null)
                    return HttpNotFound();
                else
                {
                    var viewModel = new NewOfferViewModel
                    {
                        Offer = new Offer()
                        {
                            Automobile = automobileInDb
                        }
                    };
                    return View(viewModel);
                }
            }
            else
                return HttpNotFound();
        }
        public ActionResult Save(Offer offer)
        {
            if (!ModelState.IsValid)
            {
                offer.Automobile = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift)
                    .Single(a => a.Id == offer.AutomobileId);
                var viewModel = new NewOfferViewModel
                {
                    Offer = offer
                };
                return View("NewOffer", viewModel);
            }
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                if (offer.Id == 0)
                {
                    _context.Offers.Add(offer);
                    _context.SaveChanges();
                    Session["NewOfferSucc"] = "Successfully added new offer!";
                    return RedirectToAction("ShowOffers/" + offer.AutomobileId);
                }
                else
                    return HttpNotFound();
            }
            else
                return HttpNotFound();
        }
    }
}