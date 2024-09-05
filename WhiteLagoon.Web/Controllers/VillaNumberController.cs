using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(x => x.Villa).ToList();
            return View(villaNumbers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            VillaNumberVM vm = new()
            {
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            //IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(x=> new SelectListItem { Text= x.Name, Value=x.Id.ToString() });
            //ViewData["VillaList"] = list;
            //ViewBag.VillaList = list;
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            //ModelState.Remove("Villa");
            bool roomNumberExists = _db.VillaNumbers.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists && obj.VillaNumber is not null)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                var result = _db.SaveChanges();
                if (result == 1)
                {
                    TempData["success"] = "The villa number has been created successfully.";
                }
                else
                {
                    TempData["error"] = "The villa number could not be created!";
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The villa number already exists!";
                obj.VillaList = _db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                return View(obj);
            }
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM vm = new()
            {
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
            };

            if ((vm.VillaNumber == null))
            {
                return RedirectToAction("Error", "Home");
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid && villaNumberVM.VillaNumber is not null)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                var result = _db.SaveChanges();
                if (result == 1)
                {
                    TempData["success"] = "The villa number has been updated successfully.";
                }
                else
                {
                    TempData["error"] = "The villa number could not be update!";

                }
                return RedirectToAction(nameof(Index));
            }

            villaNumberVM.VillaList = _db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM vm = new()
            {
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
            };

            if ((vm.VillaNumber == null))
            {
                return RedirectToAction("Error", "Home");
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                var result = _db.SaveChanges();
                if (result == 1)
                {
                    TempData["success"] = "The villa number has been deleted successfully.";
                }
                else
                {
                    TempData["error"] = "The villa number could not be deleted!";
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The villa number could not be deleted!";
                return View();
            }
        }
    }
}
