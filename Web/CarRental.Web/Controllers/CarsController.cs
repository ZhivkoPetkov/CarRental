using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.ViewModels.Cars;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CarRental.Web.ViewModels.Home;
using CarRental.Web.ViewModels.Reviews;

namespace CarRental.Web.Controllers
{
    public class CarsController : BaseController
    {
        private const string TimeAdded = "TimeAdded";

        private readonly ICarsService carsService;
        private readonly IMapper mapper;

        public CarsController(ICarsService carsService, IMapper mapper)
        {
            this.carsService = carsService;
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult Available(SearchCarsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var cars = this.carsService.GetAvailableCars(model.Pickup, model.Return, model.PickupPlace);

            var viewModel = new AvailableCarsViewModel
            {
                Cars = cars,
                Start = model.Pickup,
                End = model.Return,
                Days = (model.Return.Date - model.Pickup.Date).TotalDays,
                PickUpPlace = model.PickupPlace,
                ReturnPlace = model.ReturnPlace
            };

            return this.View(viewModel);
        }

        public IActionResult All(string orderBy = TimeAdded)
        {
            var cars = this.carsService.GetAllCars(orderBy);
            return this.View(cars);
        }
 
        public IActionResult Details(int id)
        {
            var car = this.carsService.FindCar(id);

            if (car is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = this.mapper.Map<CarDetailsViewModel>(car);
            viewModel.Reviews = this.mapper.Map<List<ReviewViewModel>>(car.Reviews);
            return this.View(viewModel);

        }
    }
}
