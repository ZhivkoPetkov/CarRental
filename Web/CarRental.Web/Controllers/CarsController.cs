using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.ViewModels.Cars;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Web.ViewModels.Home;
using CarRental.Web.ViewModels.Reviews;
using X.PagedList;

namespace CarRental.Web.Controllers
{
    public class CarsController : BaseController
    {
        private const string TimeAdded = "TimeAdded";
        private const int DefaultPageIndex = 1;
        private const int DefaultCarsPerPage = 9;
        private readonly ICarsService carsService;
        private readonly IMapper mapper;

        public CarsController(ICarsService carsService, IMapper mapper)
        {
            this.carsService = carsService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Available(SearchCarsViewModel model)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var cars = await this.carsService.GetAvailableCars(model.Pickup, model.Return, model.PickupPlace).ToListAsync();

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

        public async Task<IActionResult> All(int? pageNumber, int? pageSize, string orderBy = TimeAdded)
        {
            var cars = this.carsService.GetAllCars(orderBy);


            pageNumber = pageNumber ?? DefaultPageIndex;
            pageSize = pageSize ?? DefaultCarsPerPage;
            var pageProductsViewMode = await cars.ToPagedListAsync(pageNumber.Value, pageSize.Value);

            this.TempData["order"] = orderBy;
            return this.View(pageProductsViewMode);
        }
 
        public async Task<IActionResult> Details(int id)
        {
            var car = await this.carsService.FindCar(id);

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
