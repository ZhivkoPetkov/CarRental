using CarRental.Services.Contracts;
using CarRental.Web.InputModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Preview(OrderPreviewInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/");
            }

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Order(OrderInputViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/");
            }

            var result = this.ordersService.MakeOrder(this.User.Identity.Name, inputModel.Id, inputModel.PickUpPlace, inputModel.ReturnPlace,
                    inputModel.Price, inputModel.PickUp, inputModel.Return);

            if (!result)
            {
                return Redirect("/");
            }

            return Content("succesfully made order!");
        }
    }
}
