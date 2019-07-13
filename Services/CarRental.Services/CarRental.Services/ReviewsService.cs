using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarRental.Data;
using CarRental.DTOs.Reviews;
using CarRental.Models;
using CarRental.Services.Contracts;

namespace CarRental.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly CarRentalDbContext dbContext;
        private readonly IVouchersService vouchersService;
        private readonly IMapper mapper;
        private readonly IOrdersService ordersService;

        public ReviewsService(CarRentalDbContext dbContext, IVouchersService vouchersService, 
                                    IMapper mapper,IOrdersService ordersService)
        {
            this.dbContext = dbContext;
            this.vouchersService = vouchersService;
            this.mapper = mapper;
            this.ordersService = ordersService;
        }

        public async Task<bool> CreateReview(string orderId, int rating, string comment)
        {
            var order = this.dbContext.Orders.Find(orderId);

            if (order is null)
            {
                return false;
            }

            order.Review = new Review
            {
                ApplicationUserId = order.ApplicationUserId,
                CarId = order.CarId,
                Comment = comment,
                Rating = rating
            };

            var isCreatedVoucher = await this.vouchersService.CreateForUser(order.User.UserName);

            if (!isCreatedVoucher)
            {
                return false;
            }

            this.dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteReview(int id)
        {
            var review = this.dbContext.Reviews.Find(id);

            if (review is null)
            {
                return false;
            }
            await this.ordersService.DeleteReviewFromOrder(id);
            this.dbContext.Reviews.Remove(review);
            this.dbContext.SaveChanges();
            return true;
        }

        public ICollection<ListReviewDto> GetAllReviews()
        {
            var reviews = this.dbContext.Reviews.ToList();

            return this.mapper.Map<List<ListReviewDto>>(reviews);
        }
    }
}
