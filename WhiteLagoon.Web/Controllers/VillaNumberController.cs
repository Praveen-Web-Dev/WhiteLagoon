using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.Models.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;
        public VillaNumberController(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfwork.VillaNumber.GetAll(includeProperties: "Villa");   
            return View(villaNumbers);
        }

        public IActionResult Create()
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                })
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool isRoomExists = _unitOfwork.VillaNumber.Any(u => u.Villa_Number==obj.VillaNumber.Villa_Number);
            //ModelState.Remove("Villa"); one way of removing validation
            if (ModelState.IsValid && !isRoomExists)
            {
            _unitOfwork.VillaNumber.Add(obj.VillaNumber);
            _unitOfwork.Save();
                TempData["success"] = "The villa Number has been created successfully";
                return RedirectToAction("Index");
            }

            if (isRoomExists)
            {
                TempData["error"] = "The villa number already exists";

            }
            obj.VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()

            });

            return View(obj);

        }
        //Upadte Method
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                VillaNumber = _unitOfwork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)

            };
           
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
          
            if (ModelState.IsValid )
            {
                _unitOfwork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfwork.Save();
                TempData["success"] = "The villa Number has been updated successfully";
                return RedirectToAction("Index");
            }

            
            villaNumberVM.VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()

            });

            return View(villaNumberVM);
        }
        //Delete Method
        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfwork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                VillaNumber = _unitOfwork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)

            };

            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objfromDb = _unitOfwork.VillaNumber.Get(u => u.Villa_Number==villaNumberVM.VillaNumber.Villa_Number);

            if (objfromDb is not null)
            {
                _unitOfwork.VillaNumber.Remove(objfromDb);
                _unitOfwork.Save();
                TempData["success"] = "The villa Number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa couldn't be deleted";

            return View();

        }
    }

}
