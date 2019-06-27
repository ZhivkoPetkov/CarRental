namespace CarRental.Web.Controllers
{
    using CarRental.Services.Contracts;
    using CarRental.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ILocationsService locationsService;

        public HomeController(ILocationsService locationsService)
        {
            this.locationsService = locationsService;
        }

        public IActionResult Index()
        {
            var locationsList = this.locationsService.GetAllLocationNames();

            return this.View(new SearchCarsViewModel { Locations = locationsList });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
