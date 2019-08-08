using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarRental.Services.Tests
{
    public class CarsServiceTests : BaseServiceTests
    {

        private const string CarModelTestOne = "Toyota Prius";
        private const string CarModelTestTwo = "Toyota Prius";
        private const int locationIdOne = 1;
        private const int locationIdTwo = 2;
        private const int CarPricePerDayOne = 10;
        private const int CarPricePerDayTwo = 11;
        private const string CarImageTest = "image.png";
        private const string CarModelDescriptionOne = "Toyota Prius";

        private const string CarModelDescriptionTwo =
            "The car that pioneered the hybrid movement and has defined fuel-efficiency for four model generations still stands tall as an innovative green machine. Its fuel economy in our tests was a staggering 52 mpg overall—the highest we’ve ever recorded in a car that doesn’t plug in.";

        public const string OrderCarsByRentsAscending = "TimesRentAscending";
        public const string OrderCarsByRentsDescending = "TimesRentDescending";
        public const string OrderCarsByRatingDescending = "RatingDescending";
        public const string OrderCarsByLastAdded = "LastAdded";
        public const string OrderCarsByPriceAscending = "PriceAscending";
        public const string OrderCarsByPriceDescending = "PriceDescending";
        public const string ReviewComment = "This is just a testing commment for review";
        public const string LocationNameOne = "LocationNameOne";
        public const string LocationNameTwo = "LocationNameTwo";

        [Fact]
        public void AddCar_ShouldInsertValidCar()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_Insert")
                .Options;
            var dbContext = new CarRentalDbContext(options);

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

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);
            carsService.AddCar(car);

            var expected = 1;
            var result = dbContext.Cars.Count();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Delete_Should_ReturnFalseIfInvalidId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteCar")
                .Options;
            var dbContext = new CarRentalDbContext(options);

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

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);
            carsService.AddCar(car);


            var resultDelete = carsService.DeleteCar(2).GetAwaiter().GetResult();
            Assert.False(resultDelete);
        }


        [Fact]
        public void FindCar_ShouldReturnRightCar()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_FindCar")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdOne,
                    PricePerDay = locationIdOne,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var actualCountOfCars = dbContext.Cars.Count();
            Assert.Equal(2, actualCountOfCars);

            var result = carsService.FindCar(2).GetAwaiter().GetResult();
            Assert.Equal(insertCars[1].Model, result.Model);
        }

        [Fact]
        public void EditCarShould_SuccessfullyUpdateCarModel()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_EditCar")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionOne,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayTwo,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            carsService.AddCar(car);

            var expectedDescription = CarModelDescriptionOne;
            var actualResultDescription = dbContext.Cars.FirstOrDefault().Description;
            Assert.Equal(expectedDescription, actualResultDescription);

            var insertedCar = dbContext.Cars.FirstOrDefault();
            insertedCar.PricePerDay = CarPricePerDayTwo;
            insertedCar.Description = CarModelDescriptionTwo;

            carsService.EditCar(insertedCar);
            var updatedCar = carsService.FindCar(insertedCar.Id).GetAwaiter().GetResult();

            Assert.Equal(CarPricePerDayTwo, updatedCar.PricePerDay);
            Assert.Equal(CarModelDescriptionTwo, updatedCar.Description);
        }

        [Fact]
        public void ChangeLocationShould_SuccessfullyChangeLocation()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CarChangeLocation")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionOne,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayTwo,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            carsService.AddCar(car);

            var expectedLocation = locationIdOne;
            var actualResultLocation = dbContext.Cars.FirstOrDefault().LocationId;
            Assert.Equal(expectedLocation, actualResultLocation);

            carsService.ChangeLocation(car.Id, locationIdTwo);
            var updatedCar = carsService.FindCar(car.Id).GetAwaiter().GetResult();

            Assert.Equal(locationIdTwo, updatedCar.LocationId);
        }

        [Fact]
        public void ChangeLocationShould_ReturnFalseIfInvalidCarId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CarChangeLocation_InvalidCarId")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            var result = carsService.ChangeLocation(1, locationIdOne).GetAwaiter().GetResult();

            Assert.False(result);
        }

        [Fact]
        public void RentCarShould_CreateRentDaysForCar()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_RentCarDays")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            var car = new Car
            {
                Id = 1,
                Model = CarModelTestOne,
                Description = CarModelDescriptionOne,
                GearType = Models.Enums.GearType.Automatic,
                LocationId = locationIdOne,
                PricePerDay = CarPricePerDayTwo,
                Image = CarImageTest,
                Year = DateTime.UtcNow.Year
            };

            carsService.AddCar(car);

            var startDate = DateTime.UtcNow.Date;
            var endDate = startDate.AddDays(3);

            carsService.RentCar(startDate, endDate, car.Id);

            var expectedDates = new List<DateTime>();
            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                expectedDates.Add(dt);
            }

            var actualDatesCount =
                dbContext.CarRentDays.Where(x => x.CarId == car.Id && x.RentDate >= startDate).Count();

            Assert.Equal(expectedDates.Count, actualDatesCount);
        }

        [Fact]
        public void GetAllCarsShould_OrderByPriceDescending()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_OrderPriceDSC")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 1,
                Name = LocationNameOne
            });

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdOne,
                    PricePerDay = locationIdOne + 100,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo + 150,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 3,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var expectedResultIds = new List<int> {2, 1, 3};
            var actualResultIds = carsService.GetAllCars(OrderCarsByPriceDescending).Select(p => p.Id);

            Assert.Equal(expectedResultIds, actualResultIds);
        }

        [Fact]
        public void GetAllCarsShould_OrderByLastAdded()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_OrderAdded")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 1,
                Name = LocationNameOne
            });

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdOne,
                    PricePerDay = locationIdOne,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 3,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var expectedResultIds = new List<int> {3, 2, 1};
            var actualResultIds = carsService.GetAllCars(OrderCarsByLastAdded).Select(p => p.Id);

            Assert.Equal(expectedResultIds, actualResultIds);
        }

        [Fact]
        public void GetAllCarsShould_OrderByPriceAscending()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_OrderPriceASC")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 1,
                Name = LocationNameOne
            });

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdOne,
                    PricePerDay = locationIdOne + 100,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo + 150,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 3,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var expectedResultIds = new List<int> {3, 1, 2};
            var actualResultIds = carsService.GetAllCars(OrderCarsByPriceAscending).Select(p => p.Id);

            Assert.Equal(expectedResultIds, actualResultIds);
        }


        [Fact]
        public void GetAllCarsShould_OrderByTimesRentedAscending()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_OrderTimesRentedASC")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 1,
                Name = LocationNameOne
            });

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdOne,
                    PricePerDay = locationIdOne + 100,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo + 150,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 3,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var insertRents = new List<CarRentDays>
            {
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow
                },
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.AddDays(2)
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.AddDays(2)
                },
            };

            dbContext.CarRentDays.AddRange(insertRents);
            dbContext.SaveChanges();

            var expectedResultIds = new List<int> {3, 2, 1};
            var actualResultIds = carsService.GetAllCars(OrderCarsByRentsAscending).Select(p => p.Id);

            Assert.Equal(expectedResultIds, actualResultIds);
        }


        [Fact]
        public void GetAllCarsShould_OrderByTimesRentedDescending()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_OrderTimesRentedDSC")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 1,
                Name = LocationNameOne
            });

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdOne,
                    PricePerDay = locationIdOne + 100,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo + 150,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 3,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var insertRents = new List<CarRentDays>
            {
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow
                },
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.AddDays(2)
                },
                new CarRentDays
                {
                    CarId = 3,
                    RentDate = DateTime.UtcNow.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 3,
                    RentDate = DateTime.UtcNow.AddDays(2)
                },
            };

            dbContext.CarRentDays.AddRange(insertRents);
            dbContext.SaveChanges();

            var expectedResultIds = new List<int> {1, 3, 2};
            var actualResultIds = carsService.GetAllCars(OrderCarsByRentsDescending).Select(p => p.Id);

            Assert.Equal(expectedResultIds, actualResultIds);
        }


        [Fact]
        public void GetAllCarsShould_OrderByRatingDescending()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_OrderRatingDSC")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 1,
                Name = LocationNameOne
            });

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdOne,
                    PricePerDay = locationIdOne + 100,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo + 150,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 3,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var insertReviews = new List<Review>
            {
                new Review
                {
                    CarId = 1,
                    Comment = ReviewComment,
                    Rating = 4
                },
                new Review
                {
                    CarId = 1,
                    Comment = ReviewComment,
                    Rating = 3
                },
                new Review
                {
                    CarId = 2,
                    Comment = ReviewComment,
                    Rating = 5

                },
                new Review
                {
                    CarId = 3,
                    Comment = ReviewComment,
                    Rating = 5

                },
                new Review
                {
                    CarId = 3,
                    Comment = ReviewComment,
                    Rating = 4
                },
            };

            dbContext.Reviews.AddRange(insertReviews);
            dbContext.SaveChanges();

            var expectedResultIds = new List<int> {2, 3, 1};
            var actualResultIds = carsService.GetAllCars(OrderCarsByRatingDescending).Select(p => p.Id);

            Assert.Equal(expectedResultIds, actualResultIds);
        }

        [Fact]
        public void GetAvailableCarsShould_ReturnOnlyCarsWithoutRentsForLocationAndDatePeriod()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_OnlyNotRented")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description = CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = locationIdOne,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 3,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var insertRents = new List<CarRentDays>
            {
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.Date
                },
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.Date.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.Date.AddDays(2)
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.Date.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.Date.AddDays(2)
                },
                new CarRentDays
                {
                    CarId = 3,
                    RentDate = DateTime.UtcNow.Date.AddDays(10)
                },
            };

            dbContext.CarRentDays.AddRange(insertRents);
            dbContext.SaveChanges();

            var expectedResultIds = new List<int> {3};
            var actualResultIds = carsService
                .GetAvailableCars(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(3), LocationNameTwo)
                .Select(p => p.Id).ToList();

            Assert.Equal(expectedResultIds, actualResultIds);
        }


        [Fact]
        public void GetAvailableCarsShould_ReturnEmptyCollectionIfAllCarsAreRented()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_NoAvailableCars")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = locationIdOne,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 3,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var insertRents = new List<CarRentDays>
            {
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.Date.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.Date.AddDays(2)
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.Date
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.Date.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.Date.AddDays(2)
                },
                new CarRentDays
                {
                    CarId = 3,
                    RentDate = DateTime.UtcNow.Date.AddDays(1)
                },
            };

            dbContext.CarRentDays.AddRange(insertRents);
            dbContext.SaveChanges();

            var actualResultIds = carsService
                .GetAvailableCars(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1), LocationNameTwo)
                .Select(p => p.Id);

            Assert.Empty(actualResultIds);
        }

        [Fact]
        public void GetCarModelByIdShould_ReturnTheRightCarModel()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CarModel")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = locationIdOne,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());
            dbContext.SaveChanges();

            var expected = CarModelTestTwo;
            var result = carsService.GetCarModelById(insertCars[1].Id).GetAwaiter().GetResult();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetCarModelByIdShould_ReturnEmptyString()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_InvalidCarModel")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            var expected = String.Empty;
            var result = carsService.GetCarModelById(1).GetAwaiter().GetResult();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsAlreadyRentedShould_ReturnТrueIfCarIsRented()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_IsRentedCar")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = locationIdOne,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var insertRents = new List<CarRentDays>
            {
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.Date.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 1,
                    RentDate = DateTime.UtcNow.Date.AddDays(2)
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.Date
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.Date.AddDays(1)
                },
                new CarRentDays
                {
                    CarId = 2,
                    RentDate = DateTime.UtcNow.Date.AddDays(2)
                },
            };

            dbContext.CarRentDays.AddRange(insertRents);
            dbContext.SaveChanges();

            var result = carsService.IsAlreadyRented(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(2), insertCars[1].Id)
                .GetAwaiter().GetResult();

            Assert.True(result);
        }

        [Fact]
        public void IsAlreadyRentedShould_ReturnFalseIfCarIsNotRented()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_IsRentedCarFalse")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = LocationNameTwo
            });

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = locationIdOne,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());
            dbContext.SaveChanges();

            var result = carsService.IsAlreadyRented(DateTime.UtcNow, DateTime.UtcNow.AddDays(2), insertCars[0].Id)
                .GetAwaiter().GetResult();

            Assert.False(result);
        }

        [Fact]
        public void FindCarForEditShould_ReturnRightCar()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_ForEditCar")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdOne,
                    PricePerDay = locationIdOne,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var actualCountOfCars = dbContext.Cars.Count();
            Assert.Equal(2, actualCountOfCars);

            var result = carsService.FindCarForEdit(2).GetAwaiter().GetResult();
            Assert.Equal(insertCars[1].Model, result.Model);
        }

        [Fact]
        public void FindCarForEditShould_ReturnNullIfCarNotInUse()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_ForEditCar_notInUse")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = CarModelTestOne,
                    Description =
                        CarModelDescriptionOne,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdOne,
                    PricePerDay = locationIdOne,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year
                    
                },
                new Car
                {
                    Id = 2,
                    Model = CarModelTestTwo,
                    Description = CarModelDescriptionTwo,
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = locationIdTwo,
                    PricePerDay = CarPricePerDayTwo,
                    Image = CarImageTest,
                    Year = DateTime.UtcNow.Year,
                    inUse = false
                },
            };

            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var actualCountOfCars = dbContext.Cars.Count();
            Assert.Equal(2, actualCountOfCars);

            var result = carsService.FindCarForEdit(2).GetAwaiter().GetResult();
            Assert.Null(result);
        }

        [Fact]
        public void Delete_Should_ReturnTrueIfCorrectId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteValidCar")
                .Options;
            var dbContext = new CarRentalDbContext(options);

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
            dbContext.Cars.Add(car);
            dbContext.SaveChanges();

            var carsService = new CarsService(dbContext, this.cloudinary, this.mapper);
            carsService.DeleteCar(car.Id);

            var result = dbContext.Cars.Find(car.Id).inUse;

            Assert.False(result);
        }
    }
}
