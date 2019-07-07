using AutoMapper;
using CarRental.Models;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.ViewModels.Cars;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            return this.View(new AddCarViewModel { Locations = locationsList });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCarViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var car = this.mapper.Map<Car>(inputModel);
            car.Image = await this.imagesService.UploadImage(this.cloudinary, inputModel.ImageFile, inputModel.Model);
            this.carsService.AddCar(car);

            return Redirect("/");
        }
    }
}