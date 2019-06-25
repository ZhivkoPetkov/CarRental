using AutoMapper;
using CarRental.Models;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.ViewModels.Locations;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Areas.Administration.Controllers
{
    public class LocationsController : AdministrationController
    {
        private readonly ILocationsService locationsService;
        private readonly IMapper mapper;

        public LocationsController(ILocationsService locationsService, IMapper mapper)
        {
            this.locationsService = locationsService;
            this.mapper = mapper;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddLocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Content("Invalid data");
            }

            var location = mapper.Map<Location>(model);
            this.locationsService.CreateLocation(location);

            return View();
        }
    }
}