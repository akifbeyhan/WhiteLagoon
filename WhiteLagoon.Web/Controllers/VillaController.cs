using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepo;

        public VillaController(IVillaRepository villaRepo)
        {
            _villaRepo = villaRepo;
        }
        public IActionResult Index()
        {
            var villas = _villaRepo.GetAll();
            return View(villas);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description) { ModelState.AddModelError("", "The description cannot exactly match the Name."); }//Contom validation

            if (ModelState.IsValid)
            {
                _villaRepo.Add(obj);
                _villaRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            { return View(); }
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _villaRepo.Get(x => x.Id == villaId);
            if ((obj == null))
            {
                return RedirectToAction("Error", "Home");
            }
            return View("Update", obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                _villaRepo.Update(obj);
                _villaRepo.Save();
                TempData["success"] = "The villa has been updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The villa could not be updated!";
                return View();
            }
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _villaRepo.Get(x => x.Id == villaId);
            if ((obj is null))
            {
                return RedirectToAction("Error", "Home");
            }
            return View("Delete", obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _villaRepo.Get(x => x.Id == obj.Id);
            if (objFromDb is not null)
            {
                _villaRepo.Remove(objFromDb);
                _villaRepo.Save();
                TempData["success"] = "The villa has been deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The villa could not be deleted!";
                return View();
            }
        }
    }
}
