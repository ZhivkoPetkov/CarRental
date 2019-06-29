using CarRental.Services.Contracts;
using CarRental.Web.ViewModels.Cars;
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

            var cars = this.carsService.GetAvailableCars(model.Pickup, model.Return, model.PickupPlace);

            var viewModel = new AvailableCarsViewModel
            {
                Cars = cars,
                Start = model.Pickup,
                End = model.Return,
                Days = (model.Return.Date - model.Pickup.Date).TotalDays
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult All()
        {
            return this.View();
        }
    }
}
