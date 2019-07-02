using CarRental.Services.Contracts;
using CarRental.Web.InputModels.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Controllers
{
    public class ReviewsController : BaseController
    {
        private readonly IOrdersService ordersService;

        public ReviewsController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ReviewInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return View();
        }

        [Authorize]
        public IActionResult Create(string orderId)
        {
            bool isValidRequest = this.ordersService.IsValidReviewRequest(orderId, this.User.Identity.Name);

            if (!isValidRequest)
            {
                return BadRequest();
            }

            return View();
        }
    }
}