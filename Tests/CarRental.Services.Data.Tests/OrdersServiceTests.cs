using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CarRental.Common;
using CarRental.Data;
using CarRental.Models;
using CarRental.Models.Enums;
using CarRental.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarRental.Services.Tests
{
    public class OrdersServiceTests : BaseServiceTests
    {
        private const decimal OrderPrice = 100;
        private const string locationName = GlobalConstants.DefaultLocationName;
        private DateTime RentStart = DateTime.UtcNow.Date;
        private DateTime RentEnd = DateTime.UtcNow.Date.AddDays(3);
        private const string UserLastName = "LastAdmin";
        private const string UserFirstName = "Admin";
        private const string CarModelTestOne = "Toyota Prius";
        private const int ReviewId = 1;
        private const int locationIdOne = 1;
        private const int CarPricePerDayOne = 10;
        private const string CarImageTest = "image.png";
        private const string CarModelDescriptionTwo = "The car that pioneered the hybrid movement and has defined fuel-efficiency for four model generations still stands tall as an innovative green machine. Its fuel economy in our tests was a staggering 52 mpg overall—the highest we’ve ever recorded in a car that doesn’t plug in.";

        [Fact]
        public void MakeOrderShould_SuccesfullyCreateOrder()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_MakeOrder")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper, 
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            Assert.False(dbContext.Orders.Any());

            var order = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                voucher.VoucherCode);

            Assert.True(dbContext.Orders.Any(x => x.CarId == car.Id && x.ApplicationUserId == user.Id 
                                                                    && x.RentStart == RentStart && x.RentEnd == RentEnd && x.Price == OrderPrice));
        }


        [Fact]
        public void MakeOrderShould_ReturnFalseIfInvalidCustomerEmail()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_MakeOrder_InvalidEmail")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var result = ordersService.MakeOrder(Guid.NewGuid().ToString(), car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                voucher.VoucherCode).GetAwaiter().GetResult();

            Assert.False(result);
        }


        [Fact]
        public void MakeOrderShould_ReturnFalseIfInvalidLocations()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_MakeOrder_InvalidEmail")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var resultInvalidPickUpLocation = ordersService.MakeOrder(user.Email, car.Id, Guid.NewGuid().ToString(), locationName, OrderPrice, RentStart, RentEnd,
                voucher.VoucherCode).GetAwaiter().GetResult();

            Assert.False(resultInvalidPickUpLocation);


            var resultInvalidReturnLocation = ordersService.MakeOrder(user.Email, car.Id, locationName, Guid.NewGuid().ToString(), OrderPrice, RentStart, RentEnd,
                voucher.VoucherCode).GetAwaiter().GetResult();

            Assert.False(resultInvalidReturnLocation);
        }
        
        [Fact]
        public void CancelOrderShould_ChangeOrderStatusToCanceled()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CancelOrder")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var order = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                voucher.VoucherCode).GetAwaiter().GetResult();

            Assert.True(dbContext.Orders.Count() == 1 && dbContext.Orders.First().Status == OrderStatus.Active);

            var orderId = dbContext.Orders.First().Id;

            var isCanceled = ordersService.Cancel(orderId);

            Assert.True(dbContext.Orders.Count() == 1 && dbContext.Orders.First().Status == OrderStatus.Canceled);
        }


        [Fact]
        public void DeleteOrderShould_SuccessfullyDeleteOrder()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteOrder")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);
            car.RentDays.Add(new CarRentDays
            {
                CarId = car.Id,
                RentDate = DateTime.UtcNow.Date
            });

            for (var dt = RentStart; dt <= RentEnd; dt = dt.AddDays(1))
            {
                car.RentDays.Add(new CarRentDays
                {
                    CarId = car.Id,
                    RentDate = dt
                });
            }

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);

            var order = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                voucher.VoucherCode).GetAwaiter().GetResult();

            var orderId = dbContext.Orders.First().Id;

            var isDeleted = ordersService.Delete(orderId).GetAwaiter().GetResult();

            Assert.False(dbContext.Orders.Any(x => x.Id == orderId));
        }


        [Fact]
        public void DeleteAndCancelOrderShould_ReturnFalseIfInvalidOrderId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteInvalidOrder")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);

            var resultDelete = ordersService.Delete(Guid.NewGuid().ToString()).GetAwaiter().GetResult();
            Assert.False(resultDelete);

            var resultCancel = ordersService.Cancel(Guid.NewGuid().ToString()).GetAwaiter().GetResult();
            Assert.False(resultCancel);
        }


        [Fact]
        public void DeleteReviewFromOrderShould_SuccessfullyDeleteReview()
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

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var isCreatedOrder = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                voucher.VoucherCode).GetAwaiter().GetResult();

            var order = dbContext.Orders.First();

            order.Review = new Review
            {
                Id = ReviewId,
                ApplicationUserId = user.Id,
                CarId = car.Id,
                Rating = 5,
                Comment = Guid.NewGuid().ToString()
            };
            dbContext.SaveChanges();

            Assert.True(dbContext.Reviews.Any());

            ordersService.DeleteReviewFromOrder(ReviewId);

            var orderReview = dbContext.Orders.First()?.ReviewId;

            Assert.Null(orderReview);
        }


        [Fact]
        public void DeleteReviewFromOrderShould_ReturnFalseIfInvalidReviewId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteInvalidReview")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);

            var result = ordersService.DeleteReviewFromOrder(ReviewId).GetAwaiter().GetResult();
            
            Assert.False(result);
        }

        [Fact]
        public void EditOrderShould_ReturnFalseIfInvalidOrderId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_EditInvalidOrder")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);

            var result = ordersService.EditOrder(Guid.NewGuid().ToString(), null,null,null,0).GetAwaiter().GetResult();

            Assert.False(result);
        }


        [Fact]
        public void EditOrderShould_SuccessfullyEditOrder()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_EditOrder")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = UserFirstName,
                LastName = UserLastName
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var isCreated = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                voucher.VoucherCode).GetAwaiter().GetResult();

            var order = dbContext.Orders.First();

            var expected = user.FirstName + " " + user.LastName;
            var result = order.User.FirstName + " " + user.LastName;
            Assert.Equal(expected, result);

            var updatedFirstName = Guid.NewGuid().ToString();
            var updatedLastName = Guid.NewGuid().ToString();
            var updatedPrice = 100;

            ordersService.EditOrder(order.Id, updatedFirstName, updatedLastName, user.Email, updatedPrice);

            var updatedOrder = dbContext.Orders.First();

            var expectedAfterUpdate = user.FirstName + " " + user.LastName + " " + updatedPrice;
            var resultAfterUpdate = updatedOrder.User.FirstName + " " + updatedOrder.User.LastName + " " + updatedOrder.Price;
            Assert.Equal(expectedAfterUpdate, resultAfterUpdate);
        }

        [Fact]
        public void FinishOrderShould_ChangeOrderStatusToFinished()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_FinishOrder")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var dayStart = RentStart.AddDays(-5);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(dayStart, RentStart, car.Id)).
                                    ReturnsAsync(true);
            carsServiceMock.Setup(x => x.ChangeLocation(car.Id, location.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var order = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, dayStart, RentStart,
                voucher.VoucherCode).GetAwaiter().GetResult();

            Assert.True(dbContext.Orders.Count() == 1 && dbContext.Orders.First().Status == OrderStatus.Active);

            var orderId = dbContext.Orders.First().Id;

            var isFinished = ordersService.Finish(orderId);

            Assert.True(dbContext.Orders.Count() == 1 && dbContext.Orders.First().Status == OrderStatus.Finished);
        }

        [Fact]
        public void FinishOrderShould_ReturnFalseIfInvalidOrderId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_FinishInvalidOrder")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var voucher = new Voucher()
            {
                Id = 1,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = 5
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Vouchers.Add(voucher);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(RentStart, RentEnd, car.Id)).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher(voucher.VoucherCode)).
                                    ReturnsAsync(true);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var result = ordersService.Finish(Guid.NewGuid().ToString()).GetAwaiter().GetResult();

            Assert.False(result);
        }


        [Fact]
        public void GetAllOrdersForUserShould_ReturnOnlyUserOrders()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_AllOrders")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var user2 = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test2@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var car2= new Car
            {
                Id = 2,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            dbContext.Users.Add(user);
            dbContext.Users.Add(user2);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Cars.Add(car2);
            dbContext.SaveChanges();
            
            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user2.Email)).
                                    Returns(user2.Id);
            usersServiceMock.Setup(p => p.GetUserByEmail(user2.Email)).
                                    Returns(user2);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher("none")).
                                    ReturnsAsync(false);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var order1 = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                "none").GetAwaiter().GetResult();
            var order2 = ordersService.MakeOrder(user2.Email, car2.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                "none").GetAwaiter().GetResult();

            var result = ordersService.GetAllOrdersForUser(user2.Email).Count();

            Assert.Equal(1,result);
        }


        [Fact]
        public void GetAllOrdersForUserShould_ReturnEmptyCollectionIfInvalidEmail()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_AllInvalidOrders")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.SaveChanges();

            var usersServiceMock = new Mock<IUsersService>();

            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);
            usersServiceMock.Setup(p => p.GetUserByEmail(user.Email)).
                                    Returns(user);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher("none")).
                                    ReturnsAsync(false);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);

            var result = ordersService.GetAllOrdersForUser(Guid.NewGuid().ToString()).Count();

            Assert.Equal(0, result);
        }

        [Fact]
        public void UserFinishedOrdersShould_ReturnTrueIfHasFinishedOrders()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_areFinished")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin",
                UserName = "test@test.bg"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            var car2 = new Car
            {
                Id = 2,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.Cars.Add(car2);
            dbContext.SaveChanges();

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);
            usersServiceMock.Setup(p => p.GetUserByEmail(user.Email)).
                                    Returns(user);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher("none")).
                                    ReturnsAsync(false);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var order1 = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                "none").GetAwaiter().GetResult();
            var order2 = ordersService.MakeOrder(user.Email, car2.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                "none").GetAwaiter().GetResult();

            var firstOrder = dbContext.Orders.First();
            firstOrder.Status = OrderStatus.Finished;
            dbContext.SaveChanges();

            var hasFinished = ordersService.UserFinishedOrders(user.UserName);

            Assert.True(hasFinished);
        }


        [Fact]
        public void IsValidReviewRequestShould_ReturnTrueIfHisOrder()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_ValidEmailRequest")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin",
                UserName = "test@test.bg"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.SaveChanges();

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);
            usersServiceMock.Setup(p => p.GetUserByEmail(user.Email)).
                                    Returns(user);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher("none")).
                                    ReturnsAsync(false);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var isCreated = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                "none").GetAwaiter().GetResult();
         
            var order = dbContext.Orders.First();

            var isValid = ordersService.IsValidReviewRequest(order.Id, user.Email).GetAwaiter().GetResult();

            Assert.True(isValid);
        }


        [Fact]
        public void IsValidReviewRequestShould_ReturnFalseIfInvalidEmail()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_InvalidEmailRequest")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin",
                UserName = "test@test.bg"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.SaveChanges();

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);
            usersServiceMock.Setup(p => p.GetUserByEmail(user.Email)).
                                    Returns(user);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher("none")).
                                    ReturnsAsync(false);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var isCreated = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                "none").GetAwaiter().GetResult();

            var order = dbContext.Orders.First();

            var isValid = ordersService.IsValidReviewRequest(order.Id, Guid.NewGuid().ToString()).GetAwaiter().GetResult();

            Assert.False(isValid);
        }


        [Fact]
        public void GetOrderByIdShould_ReturnCorrectOrder()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_OrderById")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin",
                UserName = "test@test.bg"
            };

            var location = new Location()
            {
                Id = 1,
                Name = locationName
            };

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionTwo,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayOne,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            dbContext.Users.Add(user);
            dbContext.Locations.Add(location);
            dbContext.Cars.Add(car);
            dbContext.SaveChanges();

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                    Returns(user.Id);
            usersServiceMock.Setup(p => p.GetUserByEmail(user.Email)).
                                    Returns(user);

            var locationsServiceMock = new Mock<ILocationsService>();
            locationsServiceMock.Setup(x => x.GetIdByName(location.Name)).
                                    Returns(location.Id);

            var carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.RentCar(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).
                                    ReturnsAsync(true);

            var vouchersServiceMock = new Mock<IVouchersService>();
            vouchersServiceMock.Setup(x => x.UseVoucher("none")).
                                    ReturnsAsync(false);

            var ordersService = new OrdersService(dbContext, usersServiceMock.Object, this.mapper,
                                                    locationsServiceMock.Object, carsServiceMock.Object, vouchersServiceMock.Object);


            var isCreated = ordersService.MakeOrder(user.Email, car.Id, locationName, locationName, OrderPrice, RentStart, RentEnd,
                "none").GetAwaiter().GetResult();

            var expected = dbContext.Orders.First();
            var result = ordersService.GetOrderById(expected.Id);

            Assert.Equal(expected.Id, result.Id);
        }
    }
}
