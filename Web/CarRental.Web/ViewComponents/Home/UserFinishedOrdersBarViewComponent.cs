using CarRental.Services.Contracts;
using CarRental.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CarRental.Web.ViewComponents.Home
{
    public class UserFinishedOrdersBarViewComponent : ViewComponent
    {
        private readonly IOrdersService ordersService;

        public UserFinishedOrdersBarViewComponent(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var orders = await HasOrders();
            return View(orders);
        }

        private Task<UserFinishedOrdersBarViewModel> HasOrders()
        {
            var model = new UserFinishedOrdersBarViewModel
            {
                HasFinished = this.ordersService.UserFinishedOrders(this.User.Identity.Name)
            };

            return Task.FromResult(model);
        }
    }
}
