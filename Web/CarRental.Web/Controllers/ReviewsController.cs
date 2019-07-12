using System.Threading.Tasks;
using CarRental.Services.Contracts;
using CarRental.Web.InputModels.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Controllers
{
    public class ReviewsController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly IReviewsService reviewsService;

        public ReviewsController(IOrdersService ordersService, IReviewsService reviewsService)
        {
            this.ordersService = ordersService;
            this.reviewsService = reviewsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ReviewInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await this.reviewsService.CreateReview(inputModel.OrderId, inputModel.Rating, inputModel.Comment);

            return RedirectToAction("MyOrders", "Orders");
        }

        [Authorize]
        public IActionResult Create(string orderId)
        {
            bool isValidRequest = this.ordersService.IsValidReviewRequest(orderId, this.User.Identity.Name.ToLower()).GetAwaiter().GetResult();

            if (!isValidRequest)
            {
                return BadRequest();
            }

            ViewData["Order"] = orderId;
            return View();
        }
    }
}