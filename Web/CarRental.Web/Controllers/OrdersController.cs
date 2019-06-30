using CarRental.Web.InputModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Controllers
{
    public class OrdersController : BaseController
    {

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
    }
}
