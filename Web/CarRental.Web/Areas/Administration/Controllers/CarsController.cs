using AutoMapper;
using CarRental.Models;
using CarRental.Services.Contracts;
using CarRental.Web.ViewModels.Cars;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarRental.Web.Areas.Administration.InputModels.Cars;

namespace CarRental.Web.Areas.Administration.Controllers
{
    public class CarsController : AdministrationController
    {
        private readonly ILocationsService locationsService;
        private readonly IMapper mapper;
        private readonly ICarsService carsService;
        private readonly Cloudinary cloudinary;
        private readonly IImagesService imagesService;

        public CarsController(ILocationsService locationsService, IMapper mapper, ICarsService carsService, Cloudinary cloudinary, IImagesService imagesService)
        {
            this.locationsService = locationsService;
            this.mapper = mapper;
            this.carsService = carsService;
            this.cloudinary = cloudinary;
            this.imagesService = imagesService;
        }

        public IActionResult Add()
        {
            var locationsList = this.locationsService.GetAllLocationNames();
            return this.View(new AddCarInputModel { Locations = locationsList });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCarInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var car = this.mapper.Map<Car>(inputModel);
            car.Image = await this.imagesService.UploadImage(this.cloudinary, inputModel.ImageFile, inputModel.Model);
            await this.carsService.AddCar(car);

            return RedirectToAction("All", "Cars");
        }

        public IActionResult Edit(int id)
        {
            var car = this.carsService.FindCar(id);

            if (car is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = this.mapper.Map<CarEditViewModel>(car);
            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.carsService.DeleteCar(id);

            if (!result)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("All", "Cars");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarEditViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var car = this.mapper.Map<Car>(inputModel);

            if (inputModel.ImageFile != null)
            {
                car.Image = await this.imagesService.UploadImage(this.cloudinary, inputModel.ImageFile, inputModel.Model);
            }
            else
            {
                car.Image = inputModel.Image;
            }

            await this.carsService.EditCar(car);

            return RedirectToAction("All", "Cars");
        }
    }
}