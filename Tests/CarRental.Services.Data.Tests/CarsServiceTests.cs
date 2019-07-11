using CarRental.Data;
using CarRental.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CarRental.Services.Tests
{
    public class CarsServiceTests
    {
        [Fact]
        public void AddCarShouldContainOneCar()
        { 
        var options = new DbContextOptionsBuilder<CarRentalDbContext>()
               .UseInMemoryDatabase(databaseName: "CarRent_Database") 
               .Options;
        var dbContext = new CarRentalDbContext(options);

            dbContext.Cars.Add(new Car
            {
                Id = 11,
                Model = "Toyota Prius",
                Description = "The car that pioneered the hybrid movement and has defined fuel-efficiency for four model generations still stands tall as an innovative green machine. Its fuel economy in our tests was a staggering 52 mpg overall—the highest we’ve ever recorded in a car that doesn’t plug in.",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 7,
                PricePerDay = 39,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932389/CR-Inline-top-picks-Toyota-Prius-02-17_cvg5ta.jpg",
                Year = 2017
            });
            dbContext.SaveChangesAsync();

            var result = dbContext.Cars.Any(x => x.Model == "Toyota Prius");
            Assert.True(result);
        }


        [Fact]
        public void FindCarShouldFindExistingCar()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                   .UseInMemoryDatabase(databaseName: "CarRent_Database")
                   .Options;
            var dbContext = new CarRentalDbContext(options);

            dbContext.Cars.Add(new Car
            {
                Id = 1,
                Model = "Toyota Prius",
                Description = "The car that pioneered the hybrid movement and has defined fuel-efficiency for four model generations still stands tall as an innovative green machine. Its fuel economy in our tests was a staggering 52 mpg overall—the highest we’ve ever recorded in a car that doesn’t plug in.",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 7,
                PricePerDay = 39,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932389/CR-Inline-top-picks-Toyota-Prius-02-17_cvg5ta.jpg",
                Year = 2017
            });
            dbContext.SaveChangesAsync();

            var result = dbContext.Cars.Find(1);
            Assert.True(result != null);

            var invalidId = dbContext.Cars.Find(0);
            Assert.True(invalidId == null);
        }


        
        //bool EditCar(Car car);
        //ICollection<ListCarDto> GetAvailableCars(DateTime start, DateTime end, string location);
        //ICollection<ListCarDto> GetAllCars(string orderBy);
        //bool RentCar(DateTime start, DateTime end, int cardId);
        //CarDetailsDto FindCar(int id);
        //CarDetailsDto FindCarForEdit(int id);
        //bool ChangeLocation(int id, int returnLocationId);
    }
}
