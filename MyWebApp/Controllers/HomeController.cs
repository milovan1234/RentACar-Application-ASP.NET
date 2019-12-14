using MyWebApp.Models;
using MyWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Reflection;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private ModelContext _context;
        public HomeController()
        {
            _context = new ModelContext();
        }
        public ActionResult Index()
        {
            List<Automobile> automobiles = new List<Automobile>();
            var offers = _context.Offers.ToList();
            var autos = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift).ToList();
            for (int i = 0; i < autos.Count(); i++)
            {
                for (int j = 0; j < offers.Count(); j++)
                {
                    if (autos.ElementAt(i).Id == offers.ElementAt(j).AutomobileId)
                    {
                        automobiles.Add(autos.ElementAt(i));
                        break;
                    }
                }
            }
            var viewModel = new NewRandomViewModel
            {
                Automobile = new Automobile(),
                Automobiles = automobiles,
                SearchesAutos = automobiles,
                NumberOfDoors = _context.NumberOfDoors.ToList(),
                Gearshifts = _context.Gearshifts.ToList(),
                CarBodies = _context.CarBodies.ToList()
            };
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Search(string brand)
        {
            List<Automobile> automobiles = new List<Automobile>();
            var offers = _context.Offers.ToList();
            var autos = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift).ToList();
            for (int i = 0; i < autos.Count(); i++)
            {
                for (int j = 0; j < offers.Count(); j++)
                {
                    if (autos.ElementAt(i).Id == offers.ElementAt(j).AutomobileId)
                    {
                        automobiles.Add(autos.ElementAt(i));
                        break;
                    }
                }
            }
            if (brand != "")
            {
                var searchAutomobiles = automobiles.Where(a => a.CarBrand.ToLower().Contains(brand.ToLower())).ToList();
                var viewModel = new NewRandomViewModel
                {
                    Automobile = new Automobile(),
                    Automobiles = searchAutomobiles,
                    SearchesAutos = searchAutomobiles,
                    NumberOfDoors = _context.NumberOfDoors.ToList(),
                    Gearshifts = _context.Gearshifts.ToList(),
                    CarBodies = _context.CarBodies.ToList()
                };
                return View("Index", viewModel);
            }
            else
            {
                var viewModel = new NewRandomViewModel
                {
                    Automobile = new Automobile(),
                    Automobiles = automobiles,
                    SearchesAutos = automobiles,
                    NumberOfDoors = _context.NumberOfDoors.ToList(),
                    Gearshifts = _context.Gearshifts.ToList(),
                    CarBodies = _context.CarBodies.ToList()
                };
                return View("Index", viewModel);
            }
        }
        public ActionResult FullSearch(Automobile automobile)
        {
            var offers = _context.Offers.ToList();
            List<Automobile> cars = new List<Automobile>();
            List<Automobile> autos = new List<Automobile>();
            PropertyInfo[] properties = automobile.GetType().GetProperties();
            var listAutos = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift).ToList();           
            for (int i = 0; i < listAutos.Count(); i++)
            {
                for (int j = 0; j < offers.Count(); j++)
                {
                    if (listAutos.ElementAt(i).Id == offers.ElementAt(j).AutomobileId)
                    {
                        cars.Add(listAutos.ElementAt(i));
                        break;
                    }
                }
            }          
            foreach (Automobile auto in cars)
            {
                int count1 = 0;
                int count2 = 0;
                int countProp = 0;
                foreach (PropertyInfo property in properties)
                {
                    if (countProp < (properties.Count()-1))
                    {
                        if (property.GetValue(automobile, null) != null)
                        {
                            string propAuto = property.GetValue(automobile, null).ToString();
                            if (propAuto != "" && propAuto != "0")
                            {
                                string name = property.Name;
                                string prop = property.GetValue(auto, null).ToString();
                                if (prop.Equals(automobile[name]))
                                {
                                    count1++;
                                }
                                count2++;
                            }
                        }
                    }
                    countProp++;
                }
                if (count1 == count2)
                {
                    autos.Add(auto);
                }
            }
            var viewModel = new Object();
            if (autos.Count() > 0)
            {
                viewModel = new NewRandomViewModel
                {
                    Automobile = new Automobile(),
                    Automobiles = autos,
                    SearchesAutos = autos,
                    NumberOfDoors = _context.NumberOfDoors.ToList(),
                    Gearshifts = _context.Gearshifts.ToList(),
                    CarBodies = _context.CarBodies.ToList()
                };
            }
            else
            {
                viewModel = new NewRandomViewModel
                {
                    Automobile = new Automobile(),
                    Automobiles = autos,
                    SearchesAutos = cars,
                    NumberOfDoors = _context.NumberOfDoors.ToList(),
                    Gearshifts = _context.Gearshifts.ToList(),
                    CarBodies = _context.CarBodies.ToList()
                };
            }
            return View("Index", viewModel);
        }
    }
}