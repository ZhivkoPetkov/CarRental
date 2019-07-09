using CarRental.Common;
using CarRental.Data;
using CarRental.Models;
using CarRental.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Services
{
    public class LocationsService : ILocationsService
    {
        private readonly CarRentalDbContext dbContext;

        public LocationsService(CarRentalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
      
        public bool CreateLocation(Location location)
        {
            if (this.dbContext.
                Locations.
                Any(x => x.Name == location.Name))
            {
                return false;
            }

            this.dbContext.Locations.Add(location);
            this.dbContext.SaveChanges();
            return true;
        }

        public bool DeleteLocation(string name)
        {
            var location = this.dbContext.Locations.Where(x => x.Name == name).FirstOrDefault();

            if (location is null || location.Name.Contains(GlobalConstants.DefaultLocationName) 
                || this.dbContext.Orders.Any(p => p.PickUpLocation.Name == name))
            {
                return false;
            }

            var cars = this.dbContext.
                Cars.
                Where(x => x.LocationId == location.Id).
                ToList();

            var orders = this.dbContext.
                Orders.
                Where(x => x.ReturnLocationId == location.Id).
                ToList();

            ChangeLocationOfCar(cars);
            ChangeLocationOfOrder(orders);
            this.dbContext.Locations.Remove(location);

            this.dbContext.SaveChanges();
            return true;
        }

        private void ChangeLocationOfOrder(List<Order> orders)
        {
            foreach (var order in orders)
            {
                order.ReturnLocationId = this.GetDefaultLocation().Id;
            }
        }

        private void ChangeLocationOfCar(List<Car> cars)
        {
            foreach (var car in cars)
            {
                car.LocationId = this.GetDefaultLocation().Id;
            }
        }

        public ICollection<string> GetAllLocationNames()
        {
            return this.dbContext.Locations.
                Select(x => x.Name).
                ToList();
        }

        public int GetIdByName(string name)
        {
            return this.dbContext.
                Locations.
                FirstOrDefault(x => x.Name == name).Id;
        }

        private Location GetDefaultLocation()
        {
            return this.dbContext.
                Locations.
                Where(x => x.Name == GlobalConstants.DefaultLocationName).
                FirstOrDefault();
        }
    }
}
