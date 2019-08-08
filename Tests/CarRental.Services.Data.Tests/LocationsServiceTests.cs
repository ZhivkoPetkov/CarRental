using CarRental.Common;
using CarRental.Data;
using CarRental.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CarRental.Services.Tests
{
    public class LocationsServiceTests : BaseServiceTests
    {
        private const string locationNameOne = GlobalConstants.DefaultLocationName;
        private const string locationNameTwo = "LocationTestTwo";
        private const string locationNameThree = "LocationTestThree";

        [Fact]
        public void AddLocationShould_InsertValidLocation()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_AddLocation")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var locationsService = new LocationsService(dbContext);

            var location = new Location
            {
                Name = locationNameOne
            };

            locationsService.CreateLocation(location);

            var expected = locationNameOne;
            var result = dbContext.Locations.FirstOrDefault(x => x.Name == locationNameOne).Name;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AddLocationShould_ReturnFalseIfAlreadyInsertedName()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_AddDuplicateLocation")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var locationsService = new LocationsService(dbContext);

            var location = new Location
            {
                Name = locationNameOne
            };

            locationsService.CreateLocation(location);


            var result = locationsService.CreateLocation(location).GetAwaiter().GetResult();

            Assert.False(result);
        }

        [Fact]
        public void DeleteLocationShould_DeleteEmptyLocation()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteLocation")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var locationsService = new LocationsService(dbContext);

            var location = new Location
            {
                Name = locationNameTwo
            };
            locationsService.CreateLocation(location);

            locationsService.DeleteLocation(locationNameTwo);

            var result = dbContext.Locations.FirstOrDefault(x => x.Name == locationNameOne);

            Assert.Null(result);
        }

        [Fact]
        public void DeleteLocationShould_ReturnFalseIfTheLocationIsTheDefaultForTheSite()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteLocationDefault")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var locationsService = new LocationsService(dbContext);

            var location = new Location
            {
                Name = locationNameOne
            };
            locationsService.CreateLocation(location);

            var result = locationsService.DeleteLocation(locationNameTwo).GetAwaiter().GetResult();

            Assert.False(result);
        }

        [Fact]
        public void DeleteLocationShould_ReturnFalseIfTheLocationIsReturnPlaceForOrder()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteLocationWithOrders")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var locationsService = new LocationsService(dbContext);

            var location = new Location
            {
                Name = locationNameOne
            };
            locationsService.CreateLocation(location);

            var order = new Order
            {
                CarId = 1,
                ApplicationUserId = Guid.NewGuid().ToString(),
                PickUpLocationId = 1,
                ReturnLocationId = 1,
                Price = 100,
                RentStart = DateTime.UtcNow.Date,
                RentEnd = DateTime.UtcNow.Date.AddDays(2)
            };
            dbContext.Orders.Add(order);

            var result = locationsService.DeleteLocation(locationNameTwo).GetAwaiter().GetResult();

            Assert.False(result);
        }

        [Fact]
        public void GetAllLocationNamesShould_ReturnAllNames()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_AllLocationNames")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var locationsService = new LocationsService(dbContext);

            var locationList = new List<Location>()
            {
                new Location
                {
                    Name = locationNameOne
                },
                new Location
                {
                    Name = locationNameTwo
                },
                new Location
                {
                    Name = locationNameThree
                },
            };

            dbContext.Locations.AddRange(locationList);
            dbContext.SaveChanges();

            var expected = new List<string> {locationNameOne, locationNameTwo, locationNameThree};
            var result = locationsService.GetAllLocationNames().ToList();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetIdByNameShould_ReturnRightIdForLocation()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_IdForLocationName")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var locationsService = new LocationsService(dbContext);

            var location = new Location
            {
                Id = 1,
                Name = locationNameOne
            };

            locationsService.CreateLocation(location);

            var result = locationsService.GetIdByName(locationNameOne).GetAwaiter().GetResult();

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetIdByNameShould_ReturnZeroIfInvalidLocationName()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_0ForInvalidLocation")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var locationsService = new LocationsService(dbContext);

            var location = new Location
            {
                Id = 1,
                Name = locationNameOne
            };

            locationsService.CreateLocation(location);

            var result = locationsService.GetIdByName(locationNameThree).GetAwaiter().GetResult();

            Assert.Equal(0, result);
        }
    }
}
