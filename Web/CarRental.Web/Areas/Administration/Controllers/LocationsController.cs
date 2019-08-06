using System.Threading.Tasks;
using AutoMapper;
using CarRental.Models;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.InputModels.Locations;
using CloudinaryDotNet.Actions;
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

        public IActionResult Manage()
        {
            var locations = this.locationsService.GetAllLocationNames();

            return View(new AddLocationInputModel { Locations = locations });
        }

        public async Task<IActionResult> Delete(string name)
        {
            await this.locationsService.DeleteLocation(name);

            return RedirectToAction(nameof(Manage));
        }


        [HttpPost]
        public IActionResult Manage(AddLocationInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return Content("Invalid data");
            }

            var location = mapper.Map<Location>(inputModel);
            var result = this.locationsService.CreateLocation(location).GetAwaiter().GetResult();

            return RedirectToAction(nameof(Manage));
        }
    }
}