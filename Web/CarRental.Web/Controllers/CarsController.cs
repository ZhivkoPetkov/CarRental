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
                return RedirectToAction("Index", "Home", model);
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

        public IActionResult All(string orderBy = "timeAdded")
        {
            var cars = this.carsService.GetAllCars(orderBy);
            return this.View(cars);
        }
 
        public IActionResult Details(int id)
        {
            try
            {
                var car = this.carsService.FindCar(id);
                var viewModel = this.mapper.Map<CarDetailsViewModel>(car);
                viewModel.Reviews = this.mapper.Map<List<ReviewViewModel>>(car.Reviews);
                return this.View(viewModel);
            }
            catch (System.Exception)
            {
                throw new System.Exception("Invalid Car Id");
            }

        }
    }
}
