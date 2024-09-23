using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfwork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfwork.VillaNumber.GetAll(includeProperties:"Villa");
            return View(villaNumbers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            VillaNumberVM vm = new()
            {
                VillaList = _unitOfwork.Villa.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
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
            bool roomNumberExists = _unitOfwork.VillaNumber.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists && obj.VillaNumber is not null)
            {
                _unitOfwork.VillaNumber.Add(obj.VillaNumber);
                _unitOfwork.Save();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The villa number already exists!";
                obj.VillaList = _unitOfwork.Villa.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                return View(obj);
            }
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM vm = new()
            {
                VillaList = _unitOfwork.Villa.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = _unitOfwork.VillaNumber.Get(x => x.Villa_Number == villaNumberId)
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
                _unitOfwork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfwork.Save();
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            villaNumberVM.VillaList = _unitOfwork.Villa.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM vm = new()
            {
                VillaList = _unitOfwork.Villa.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = _unitOfwork.VillaNumber.Get(x => x.Villa_Number == villaNumberId)
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
            VillaNumber? objFromDb = _unitOfwork.VillaNumber.Get(x => x.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _unitOfwork.VillaNumber.Remove(objFromDb);
                _unitOfwork.Save();
                TempData["success"] = "The villa number has been deleted successfully.";
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
