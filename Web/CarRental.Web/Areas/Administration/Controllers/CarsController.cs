using AutoMapper;
using CarRental.Models;
using CarRental.Models.Enums;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.DTOs.Cars;
using CarRental.Web.Areas.Administration.ViewModels.Cars;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarRental.Web.Areas.Administration.Controllers
{
    public class CarsController : AdministrationController
    {
        private readonly ILocationsService locationsService;
        private readonly IMapper mapper;
        private readonly ICarsService carsService;

        public CarsController(ILocationsService locationsService, IMapper mapper, ICarsService carsService)
        {
            this.locationsService = locationsService;
            this.mapper = mapper;
            this.carsService = carsService;
        }

        public IActionResult Add()
        {
            var locationsList = this.locationsService.GetAllLocationNames();
            return this.View(new AddCarViewModel { Locations = locationsList });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCarViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            var carDto = new CarDto
            {
                Model = inputModel.Model,
                Description = inputModel.Description,
                Year = inputModel.Year,
                Image = inputModel.Image,
                PricePerDay = inputModel.PricePerDay,
                GearType = Enum.Parse<GearType>(inputModel.GearType),
                LocationId = this.locationsService.GetIdByName(inputModel.Location)
            };

            var car = this.mapper.Map<Car>(carDto);
            this.carsService.AddCar(car);

            return Redirect("/");
        }
    }
}