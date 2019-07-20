using CarRental.Data;
using CarRental.Models;
using CarRental.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CarRental.Services.Tests
{
    public class ReviewsServiceTests : BaseServiceTests
    {
        [Fact]
        public void CreateReviewShould_SuccessfullyCreateReview()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CreateReview")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.CreateForUser(user.Email)).
                                    ReturnsAsync(true);

            var ordersServiceMock = new Mock<IOrdersService>();
            ordersServiceMock.Setup(x => x.DeleteReviewFromOrder(It.IsAny<int>())).
                                    ReturnsAsync(true);

            var reviewsService = new ReviewsService(dbContext, vouchersServiceMock.Object, this.mapper, ordersServiceMock.Object);

            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                CarId = 1,
                PickUpLocationId = 1,
                ReturnLocationId = 1,
                Price = 1,
            };
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            var random = new Random();
            var reviewRating = random.Next(1, 5);
            var reviewComment = Guid.NewGuid().ToString();

            Assert.False(dbContext.Reviews.Any());

            var isCreatedReview = reviewsService.CreateReview(order.Id, reviewRating, reviewComment);

            Assert.True(order.Review != null);
        }


        [Fact]
        public void CreateReviewShould_ReturnFalseIfInvalidOrderId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CreateReviewInvalidOrder")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.CreateForUser(user.Email)).
                                    ReturnsAsync(true);

            var ordersServiceMock = new Mock<IOrdersService>();
            ordersServiceMock.Setup(x => x.DeleteReviewFromOrder(It.IsAny<int>())).
                                    ReturnsAsync(true);

            var reviewsService = new ReviewsService(dbContext, vouchersServiceMock.Object, this.mapper, ordersServiceMock.Object);

            var random = new Random();
            var reviewRating = random.Next(1, 5);
            var reviewComment = Guid.NewGuid().ToString();

            var result = reviewsService.CreateReview(Guid.NewGuid().ToString(), reviewRating, reviewComment).GetAwaiter().GetResult();

            Assert.False(result);
        }

        [Fact]
        public void DeleteReviewShould_SuccesfullyDeleteReviewFromOrder()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
              .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteReview")
              .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.CreateForUser(user.Email)).
                                    ReturnsAsync(true);

            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                CarId = 1,
                PickUpLocationId = 1,
                ReturnLocationId = 1,
                Price = 1,
            };
            dbContext.Orders.Add(order);

            var random = new Random();
            var reviewRating = random.Next(1, 5);
            var reviewComment = Guid.NewGuid().ToString();

            order.Review = new Review
            {
                Comment = reviewComment,
                Rating = reviewRating,
                Id = 1,
                CarId = 1
            };
            dbContext.SaveChanges();

            var ordersServiceMock = new Mock<IOrdersService>();
            ordersServiceMock.Setup(x => x.DeleteReviewFromOrder(order.Review.Id)).
                                    ReturnsAsync(true);

            var reviewsService = new ReviewsService(dbContext, vouchersServiceMock.Object, this.mapper, ordersServiceMock.Object);

            Assert.True(order.Review != null);

            reviewsService.DeleteReview(order.Review.Id);

            var result = dbContext.Orders.FirstOrDefault(x => x.Id == order.Id).Review == null;

            Assert.True(result);
        }

        [Fact]
        public void DeleteReviewShould_ReturnFalseIfInvalidReviewId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
              .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteReviewInvalidId")
              .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.CreateForUser(user.Email)).
                                    ReturnsAsync(true);

            var ordersServiceMock = new Mock<IOrdersService>();
            ordersServiceMock.Setup(x => x.DeleteReviewFromOrder(It.IsAny<int>())).
                                    ReturnsAsync(true);

            var random = new Random();
            var reviewsService = new ReviewsService(dbContext, vouchersServiceMock.Object, this.mapper, ordersServiceMock.Object);

            var result = reviewsService.DeleteReview(random.Next(1, 10)).GetAwaiter().GetResult();

            Assert.False(result);
        }


        [Fact]
        public void GetAllReviewsShould_ReturnAllReviewsFromCustomers()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
              .UseInMemoryDatabase(databaseName: "CarRental_Database_AllReviews")
              .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.CreateForUser(user.Email)).
                                    ReturnsAsync(true);

            var ordersServiceMock = new Mock<IOrdersService>();
            ordersServiceMock.Setup(x => x.DeleteReviewFromOrder(It.IsAny<int>())).
                                    ReturnsAsync(true);

            var random = new Random();
            var reviewsService = new ReviewsService(dbContext, vouchersServiceMock.Object, this.mapper, ordersServiceMock.Object);

            var ordersToInsert = 10;

            for (int i = 1; i <= ordersToInsert; i++)
            {
                var order = new Order
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationUserId = user.Id,
                    CarId = 1,
                    PickUpLocationId = 1,
                    ReturnLocationId = 1,
                    Price = 1,
                };

                var reviewRating = random.Next(1, 5);
                var reviewComment = Guid.NewGuid().ToString();

                order.Review = new Review
                {
                    Comment = reviewComment,
                    Rating = reviewRating,
                    Id = i,
                    CarId = 1
                };

                dbContext.Orders.Add(order);
                dbContext.SaveChanges();
            }

            var expected = ordersToInsert;
            var result = reviewsService.GetAllReviews().Count();

            Assert.Equal(expected, result);
        }
    }
}
