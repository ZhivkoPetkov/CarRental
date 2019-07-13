using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private const string CarModelDescriptionTwo = "The car that pioneered the hybrid movement and has defined fuel-efficiency for four model generations still stands tall as an innovative green machine. Its fuel economy in our tests was a staggering 52 mpg overall—the highest we’ve ever recorded in a car that doesn’t plug in.";
        public const string OrderCarsByRentsAscending = "TimesRentAscending";
        public const string OrderCarsByRentsDescending = "TimesRentDescending";
        public const string OrderCarsByRatingDescending = "RatingDescending";
        public const string OrderCarsByLastAdded = "LastAdded";
        public const string OrderCarsByPriceAscending = "PriceAscending";
        public const string OrderCarsByPriceDescending = "PriceDescending";
        [Fact]
        public void AddCar_ShouldInsertValidCar()
        {
            this.dbContext.Database.EnsureDeleted();

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

            var carsService = new CarsService(this.dbContext, this.cloudinary, this.mapper);
            carsService.AddCar(car);

            var expected = 1;
            var result = dbContext.Cars.Count();

            Assert.Equal(expected, result);

        }

        [Fact]
        public void FindCar_ShouldReturnRightCar()
        {
            this.dbContext.Database.EnsureDeleted();

            var carsService = new CarsService(this.dbContext, this.cloudinary, this.mapper);

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

            var actualCountOfCars = this.dbContext.Cars.Count();
            Assert.Equal(2, actualCountOfCars);

            var result = carsService.FindCar(2);
            Assert.Equal(insertCars[1].Model, result.Model);
        }

        [Fact]
        public void EditCarShould_SuccessfullyUpdateCarModel()
        {
            this.dbContext.Database.EnsureDeleted();
            var carsService = new CarsService(this.dbContext, this.cloudinary, this.mapper);

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
            var actualResultDescription = this.dbContext.Cars.FirstOrDefault().Description;
            Assert.Equal(expectedDescription, actualResultDescription);

            var insertedCar = this.dbContext.Cars.FirstOrDefault();
            insertedCar.PricePerDay = CarPricePerDayTwo;
            insertedCar.Description = CarModelDescriptionTwo;

            carsService.EditCar(insertedCar);
            var updatedCar = carsService.FindCar(insertedCar.Id);

            Assert.Equal(CarPricePerDayTwo, updatedCar.PricePerDay);
            Assert.Equal(CarModelDescriptionTwo, updatedCar.Description);
        }

        [Fact]
        public void ChangeLocationShould_SuccessfullyChangeLocation()
        {
            this.dbContext.Database.EnsureDeleted();
            var carsService = new CarsService(this.dbContext, this.cloudinary, this.mapper);

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
            var actualResultLocation = this.dbContext.Cars.FirstOrDefault().LocationId;
            Assert.Equal(expectedLocation, actualResultLocation);

            carsService.ChangeLocation(car.Id, locationIdTwo);
            var updatedCar = carsService.FindCar(car.Id);

            Assert.Equal(locationIdTwo, updatedCar.LocationId);
        }

        [Fact]
        public void RentCarShould_CreateRentDaysForCar()
        {
            this.dbContext.Database.EnsureDeleted();
            var carsService = new CarsService(this.dbContext, this.cloudinary, this.mapper);

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

            var actualDatesCount = dbContext.CarRentDays.
                                        Where(x => x.CarId == car.Id && x.RentDate >= startDate).
                                        Count();

            Assert.Equal(expectedDates.Count, actualDatesCount);
        }

        [Fact]
        public void GetAllCarsShould_OrderByPriceDescending()
        {
            this.dbContext.Database.EnsureDeleted();

            var carsService = new CarsService(this.dbContext, this.cloudinary, this.mapper);

            dbContext.Locations.Add(new Location
            {
                Id = 1,
                Name = "TestLocationOne"
            });

            dbContext.Locations.Add(new Location
            {
                Id = 2,
                Name = "TestLocationTwo"
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

        //TODO: add more all cars methods;
        //ICollection<ListCarDto> GetAvailableCars(DateTime start, DateTime end, string location);
       
    }
}
