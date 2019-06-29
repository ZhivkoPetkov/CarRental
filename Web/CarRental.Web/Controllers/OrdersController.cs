using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Controllers
{
    public class OrdersController : BaseController
    { 
        [Authorize]
        public IActionResult Order()
        {
            return this.View();
        }
    }
}
