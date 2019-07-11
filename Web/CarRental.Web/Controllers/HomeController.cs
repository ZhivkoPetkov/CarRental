namespace CarRental.Web.Controllers
{
    using CarRental.Services.Contracts;
    using CarRental.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ILocationsService locationsService;
        private readonly IOrdersService ordersService;

        public HomeController(ILocationsService locationsService, IOrdersService ordersService)
        {
            this.locationsService = locationsService;
            this.ordersService = ordersService;
        }

        public IActionResult Index()
        {
            var locationsList = this.locationsService.GetAllLocationNames();

            ViewData["FinishedOrders"] = this.ordersService.UserFinishedOrders(User.Identity.Name);

            return this.View(new SearchCarsViewModel { Locations = locationsList });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
