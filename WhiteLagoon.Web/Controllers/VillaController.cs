using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfwork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfwork.Villa.GetAll();
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
                #region image upload
                if(obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"\images\RoomImages\Villa\");

                    using (var fileStream = new FileStream(Path.Combine(imagePath,fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                        obj.ImageUrl = @"\images\RoomImages\Villa\" + fileName;
                    }
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }
                #endregion

                _unitOfwork.Villa.Add(obj);
                _unitOfwork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            { return View(); }
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfwork.Villa.Get(x => x.Id == villaId);
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
                #region image upload
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"\images\RoomImages\Villa\");

                    if(!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath + imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                        obj.ImageUrl = @"\images\RoomImages\Villa\" + fileName;
                    }
                }
                #endregion

                _unitOfwork.Villa.Update(obj);
                _unitOfwork.Save();
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
            Villa? obj = _unitOfwork.Villa.Get(x => x.Id == villaId);
            if ((obj is null))
            {
                return RedirectToAction("Error", "Home");
            }
            return View("Delete", obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfwork.Villa.Get(x => x.Id == obj.Id);
            
            if (objFromDb is not null)
            {
                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfwork.Villa.Remove(objFromDb);
                _unitOfwork.Save();
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
