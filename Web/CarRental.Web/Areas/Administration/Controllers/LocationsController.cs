using AutoMapper;
using CarRental.Models;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.ViewModels.Locations;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult Add(AddLocationViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return Content("Invalid data");
            }

            var location = mapper.Map<Location>(inputModel);
            var result = this.locationsService.CreateLocation(location);

            if (!result)
            {
                return Content("Invalid data, duplicate name");
            }

            return View();
        }
    }
}