using CarRental.Data;
using CarRental.Models;
using CarRental.Services.Contracts;

namespace CarRental.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly CarRentalDbContext dbContext;
        private readonly IVouchersService vouchersService;

        public ReviewsService(CarRentalDbContext dbContext, IVouchersService vouchersService)
        {
            this.dbContext = dbContext;
            this.vouchersService = vouchersService;
        }

        public bool CreateReview(string orderId, int rating, string comment)
        {
            var order = this.dbContext.Orders.Find(orderId);
            order.Review = new Review
            {
                ApplicationUserId = order.ApplicationUserId,
                CarId = order.CarId,
                Comment = comment,
                Rating = rating
            };
            this.vouchersService.CreateForUser(order.User.UserName);
            this.dbContext.SaveChanges();
            return true;
        }
    }
}
