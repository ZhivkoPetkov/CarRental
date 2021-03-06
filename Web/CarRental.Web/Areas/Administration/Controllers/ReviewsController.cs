﻿using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.ViewModels.Reviews;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

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

        public async Task<IActionResult> All()
        {
            var reviews = await this.reviewsService.GetAllReviews().ToListAsync();
            var viewModels = this.mapper.Map<List<ListReviewViewModel>>(reviews);

            return this.View(viewModels);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.reviewsService.DeleteReview(id);

            if (!isDeleted)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(All));
        }
    }
}