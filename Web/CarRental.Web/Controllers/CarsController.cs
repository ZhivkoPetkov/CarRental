using CarRental.Services.Contracts;
using CarRental.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Controllers
{
    public class CarsController : BaseController
    {
        private readonly ICarsService carsService;

        public CarsController(ICarsService carsService)
        {
            this.carsService = carsService;
        }

        [HttpPost]
        public IActionResult Available(SearchCarsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home", model);
            }

            return Content("Searched");
        }

        [HttpPost]
        public IActionResult All()
        {
            return this.View();
        }
    }
}
