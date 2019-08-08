using System.Threading.Tasks;
using AutoMapper;
using CarRental.Models;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.InputModels.Locations;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

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

        public async Task<IActionResult> Manage()
        {
            var locations = await this.locationsService.GetAllLocationNames().ToListAsync();

            return View(new AddLocationInputModel { Locations = locations });
        }

        public async Task<IActionResult> Delete(string name)
        {
            await this.locationsService.DeleteLocation(name);

            return RedirectToAction(nameof(Manage));
        }


        [HttpPost]
        public async Task<IActionResult> Manage(AddLocationInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return Content("Invalid data");
            }

            var location = mapper.Map<Location>(inputModel);
            var result = await this.locationsService.CreateLocation(location);

            return RedirectToAction(nameof(Manage));
        }
    }
}