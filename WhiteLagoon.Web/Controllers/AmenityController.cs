using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.Models.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;
        public AmenityController(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public IActionResult Index()
        {
            var Amenitys = _unitOfwork.Amenity.GetAll(includeProperties: "Villa");   
            return View(Amenitys);
        }

        public IActionResult Create()
        {

            AmenityVM AmenityVM = new()
            {
                VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                })
            };

            return View(AmenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
          
            //ModelState.Remove("Villa"); one way of removing validation
            if (ModelState.IsValid )
            {
            _unitOfwork.Amenity.Add(obj.Amenity);
            _unitOfwork.Save();
                TempData["success"] = "The amenity has been created successfully";
                return RedirectToAction("Index");
            }

           
            obj.VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()

            });

            return View(obj);

        }
        //Upadte Method
        public IActionResult Update(int AmenityId)
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                Amenity = _unitOfwork.Amenity.Get(u => u.Id == AmenityId)

            };
           
            if (AmenityVM.Amenity == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(AmenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM AmenityVM)
        {
          
            if (ModelState.IsValid )
            {
                _unitOfwork.Amenity.Update(AmenityVM.Amenity);
                _unitOfwork.Save();
                TempData["success"] = "The amenity has been updated successfully";
                return RedirectToAction("Index");
            }

            
            AmenityVM.VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()

            });

            return View(AmenityVM);
        }
        //Delete Method
        public IActionResult Delete(int AmenityId)
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                Amenity = _unitOfwork.Amenity.Get(u => u.Id == AmenityId)

            };

            if (AmenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(AmenityVM);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM AmenityVM)
        {
            Amenity? objfromDb = _unitOfwork.Amenity.Get(u => u.Id==AmenityVM.Amenity.Id);

            if (objfromDb is not null)
            {
                _unitOfwork.Amenity.Remove(objfromDb);
                _unitOfwork.Save();
                TempData["success"] = "The amenity has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity couldn't be deleted";

            return View();

        }
    }

}
