using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWebApp.ViewModels;
using System.IO;
using System.Data.Entity;

namespace MyWebApp.Controllers
{
    public class AutomobilesController : Controller
    {
        private ModelContext _context;
        public AutomobilesController()
        {
            _context = new ModelContext();
        }
        public ActionResult New()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var viewModel = new NewAutomobileViewModel
                {
                    Automobile = new Automobile(),
                    NumberOfDoors = _context.NumberOfDoors.ToList(),
                    Gearshifts = _context.Gearshifts.ToList(),
                    CarBodies = _context.CarBodies.ToList()
                };
                Session["New"] = "New";
                return View("NewAutomobile", viewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult Save(Automobile automobile, HttpPostedFileBase CarImage)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new NewAutomobileViewModel
                {
                    Automobile = new Automobile(),
                    NumberOfDoors = _context.NumberOfDoors.ToList(),
                    CarBodies = _context.CarBodies.ToList(),
                    Gearshifts = _context.Gearshifts.ToList(),
                };
                Session["CarMessage"] = null;
                return View("NewAutomobile", viewModel);
            }
            if (automobile.Id == 0)
            {
                if (CarImage != null)
                {
                    string filename = Path.GetFileName(CarImage.FileName);
                    automobile.ImagePath = "/Images/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                    CarImage.SaveAs(filename);
                }
                else
                {
                    automobile.ImagePath = "/Images/template-car.jpg";
                }
                _context.Automobiles.Add(automobile);
                Session["CarMessage"] = "Auto successfully added.";
            }
            else
            {
                var automobileInDb = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift)
                    .Single(a => a.Id == automobile.Id);

                automobileInDb.CarBrand = automobile.CarBrand;
                automobileInDb.CarModel = automobile.CarModel;
                automobileInDb.ProductYear = automobile.ProductYear;
                automobileInDb.Cubicase = automobile.Cubicase;
                automobileInDb.NumberOfDoorId = automobile.NumberOfDoorId;
                automobileInDb.CarBodyId = automobile.CarBodyId;
                automobileInDb.GearshiftId = automobile.GearshiftId;
                if (CarImage != null)
                {
                    string filename = Path.GetFileName(CarImage.FileName);
                    automobileInDb.ImagePath = "/Images/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                    CarImage.SaveAs(filename);
                }
                Session["CarMessage"] = "Auto successfully updated.";                
            }
            _context.SaveChanges();
            return RedirectToAction("New", "Automobiles");
        }
        public ActionResult Edit(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                Session["New"] = null;
                var automobile = _context.Automobiles.SingleOrDefault(a => a.Id == id);
                if (automobile == null)
                {
                    return HttpNotFound();
                }
                var viewModel = new NewAutomobileViewModel
                {
                    Automobile = automobile,
                    NumberOfDoors = _context.NumberOfDoors.ToList(),
                    Gearshifts = _context.Gearshifts.ToList(),
                    CarBodies = _context.CarBodies.ToList()
                };
                Session["UpdateAuto"] = "True";
                return View("NewAutomobile", viewModel);
            }
            else
                return HttpNotFound();
        }

        public ActionResult DeleteAutomobile(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var automobile = _context.Automobiles.SingleOrDefault(a => a.Id == id);
                if (automobile == null)
                    return HttpNotFound();
                _context.Automobiles.Remove(automobile);
                _context.SaveChanges();
                return RedirectToAction("AllAutomobiles");
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult AllAutomobiles()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                var automobiles = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift)
                    .ToList();
                var viewModel = new NewRandomViewModel
                {
                    Automobiles = automobiles
                };
                return View("Index", viewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public JsonResult ModelForSpecialBrand(string brand)
        {
            var list = _context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift)
                    .Where(a => a.CarBrand == brand).ToList();
            if (brand == "")
            {
                return Json(new SelectList(_context.Automobiles
                    .Include(a => a.NumberOfDoor)
                    .Include(a => a.CarBody)
                    .Include(a => a.Gearshift).Select(a => a.CarModel).ToList(), "CarModel"),JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new SelectList(list.Select(a => a.CarModel), "CarModel"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutomobilesPage(int id,string brand)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "Admin")
            {
                List<Automobile> autos = new List<Automobile>();
                if (brand == "" || brand == null)
                {
                    autos = _context.Automobiles.Include(a => a.CarBody).Include(a => a.NumberOfDoor).Include(a => a.Gearshift).ToList();
                    brand = "";
                }
                else if (brand != "" || brand != null)
                {
                    autos = _context.Automobiles.Where(a => a.CarBrand.Contains(brand))
                        .Include(a => a.CarBody)
                        .Include(a => a.NumberOfDoor)
                        .Include(a => a.Gearshift).ToList();
                }
                int start = 0;
                int end = 0;
                int count = 0;
                if (autos.Count() % 2 == 0)
                    count = autos.Count() / 2;
                else
                    count = (autos.Count() / 2) + 1;
                if (id == 1)
                {
                    if (autos.Count() >= 2)
                    {
                        start = 0;
                        end = 2;
                        List<Automobile> trueAutos = new List<Automobile>();
                        for (int i = start; i < end; i++)
                        {
                            trueAutos.Add(autos.ElementAt(i));
                        }
                        var viewModel = new NewRandomViewModel
                        {
                            Automobiles = trueAutos,
                            countPage = count,
                            NumberOfPage = id,
                            Brand = brand
                        };
                        return View(viewModel);
                    }
                    else if (autos.Count < 2)
                    {
                        start = 0;
                        end = autos.Count();
                        List<Automobile> trueAutos = new List<Automobile>();
                        for (int i = start; i < end; i++)
                        {
                            trueAutos.Add(autos.ElementAt(i));
                        }
                        var viewModel = new NewRandomViewModel
                        {
                            Automobiles = trueAutos,
                            countPage = count,
                            NumberOfPage = id,
                            Brand = brand
                        };
                        return View(viewModel);
                    }
                }
                else if (id > 1 && id <= count)
                {
                    if (autos.Count() >= id * 2)
                    {
                        start = (id * 2) - 2;
                        end = (id * 2);
                        List<Automobile> trueAutos = new List<Automobile>();
                        for (int i = start; i < end; i++)
                        {
                            trueAutos.Add(autos.ElementAt(i));
                        }
                        var viewModel = new NewRandomViewModel
                        {
                            Automobiles = trueAutos,
                            countPage = count,
                            NumberOfPage = id,
                            Brand = brand
                        };
                        return View(viewModel);
                    }
                    else if (autos.Count() < id * 2)
                    {
                        start = (id * 2) - 2;
                        end = autos.Count();
                        List<Automobile> trueAutos = new List<Automobile>();
                        for (int i = start; i < end; i++)
                        {
                            trueAutos.Add(autos.ElementAt(i));
                        }
                        var viewModel = new NewRandomViewModel
                        {
                            Automobiles = trueAutos,
                            countPage = count,
                            NumberOfPage = id,
                            Brand = brand
                        };
                        return View(viewModel);
                    }
                }
            }
            return HttpNotFound();
        }
    }
}