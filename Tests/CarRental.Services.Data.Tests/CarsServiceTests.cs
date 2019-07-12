using System.Collections.Generic;
using CarRental.Common;
using CarRental.Models;
using CloudinaryDotNet;
using System.Linq;
using Xunit;

namespace CarRental.Services.Tests
{
    public class CarsServiceTests : BaseServiceTests
    {

        [Fact]
        public void AddCarShouldInsertValidCar()
        {

            var car = new Car
            {
                Id = 1,
                Model = "Toyota Prius",
                Description = "The car that pioneered the hybrid movement and has defined fuel-efficiency for four model generations still stands tall as an innovative green machine. Its fuel economy in our tests was a staggering 52 mpg overall—the highest we’ve ever recorded in a car that doesn’t plug in.",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 7,
                PricePerDay = 39,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932389/CR-Inline-top-picks-Toyota-Prius-02-17_cvg5ta.jpg",
                Year = 2017
            };

            var carsService = new CarsService(this.dbContext, this.cloudinary, this.mapper);

            carsService.AddCar(car);

            Assert.True(dbContext.Cars.Count() == 1);

        }


        [Fact]
        public void FindCarShouldReturnRightCar()
        {
            this.dbContext.Database.EnsureDeleted();

            var carsService = new CarsService(this.dbContext, this.cloudinary, this.mapper);

            var insertCars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Model = "Toyota Prius",
                    Description =
                        "This is a test description",
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = 7,
                    PricePerDay = 39,
                    Image =
                        "ThisIsATestImage",
                    Year = 2017
                },
                new Car
                {
                    Id = 2,
                    Model = "BMW X5",
                    Description =
                        "This is a test description",
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = 1,
                    PricePerDay = 40,
                    Image =
                        "ThisIsATestImage",
                    Year = 2018
                }
            };


            insertCars.ForEach(x => carsService.AddCar(x).GetAwaiter().GetResult());

            var actualCountOfCars = this.dbContext.Cars.Count();
            Assert.Equal(2, actualCountOfCars);

            var result = carsService.FindCar(2);
            Assert.Equal(insertCars[1].Model, result.Model);


        }
        //CarsService(CarRentalDbContext dbContext, Cloudinary cloudinary, IMapper mapper)

        //bool EditCar(Car car);
        //ICollection<ListCarDto> GetAvailableCars(DateTime start, DateTime end, string location);
        //ICollection<ListCarDto> GetAllCars(string orderBy);
        //bool RentCar(DateTime start, DateTime end, int cardId);
        //bool ChangeLocation(int id, int returnLocationId);
    }
}
