using System;
using System.Diagnostics;
using CarRental.Web.Hubs;
using CarRental.Web.ViewModels;
using CarRental.Web.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CarRental.Web.Controllers
{
    using CarRental.Services.Contracts;
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


        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
                { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
