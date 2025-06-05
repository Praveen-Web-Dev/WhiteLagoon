using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfwork unitOfwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfwork = unitOfwork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfwork.Villa.GetAll();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Image !=null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Villa");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                    obj.ImageUrl = @"\images\Villa\" + fileName;
                }
                else
                {
                    obj.ImageUrl = "htpps://placehold.co/600x400";
                }
                    _unitOfwork.Villa.Add(obj);
                _unitOfwork.Save();
                TempData["success"] = "The villa has been created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa couldn't be created";

            return View();

        }
        //Upadte Method
        public IActionResult Update(int villaId)
        {
           Villa? obj = _unitOfwork.Villa.Get(u=>u.Id==villaId);
            if (obj == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id>0)
            {
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Villa");

                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)){
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                    obj.ImageUrl = @"\images\Villa\" + fileName;
                }
                
                _unitOfwork.Villa.Update(obj);
                _unitOfwork.Save();
                TempData["success"] = "The villa has been updated successfully";

                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa couldn't be updated";

            return View();

        }
        //Delete Method
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfwork.Villa.Get(u => u.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objfromDb = _unitOfwork.Villa.Get(u => u.Id == obj.Id);

            if (objfromDb is not null)
            {
                if (!string.IsNullOrEmpty(objfromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objfromDb.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfwork.Villa.Remove(objfromDb);
                _unitOfwork.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa couldn't be deleted";

            return View();

        }
    }

}
