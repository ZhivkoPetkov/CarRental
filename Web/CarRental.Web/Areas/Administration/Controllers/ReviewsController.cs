using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.ViewModels.Rewiews;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Areas.Administration.Controllers
{
    public class ReviewsController : AdministrationController
    {
        private readonly IReviewsService reviewsService;
        private readonly IMapper mapper;

        public ReviewsController(IReviewsService reviewsService, IMapper mapper)
        {
            this.reviewsService = reviewsService;
            this.mapper = mapper;
        }

        public IActionResult All()
        {
            var reviews = this.reviewsService.GetAllReviews();
            var viewModels = this.mapper.Map<List<ListReviewViewModel>>(reviews);

            return this.View(viewModels);
        }

        public IActionResult Delete(int id)
        {
            var result = this.reviewsService.DeleteReview(id);
            return RedirectToAction(nameof(All));
        }
    }
}