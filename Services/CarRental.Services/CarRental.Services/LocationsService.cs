using CarRental.Common;
using CarRental.Data;
using CarRental.Models;
using CarRental.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Services
{
    public class LocationsService : ILocationsService
    {
        private readonly CarRentalDbContext dbContext;

        public LocationsService(CarRentalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
      
        public async Task<bool> CreateLocation(Location location)
        {
            if (this.dbContext.
                Locations.
                Any(x => x.Name == location.Name))
            {
                return false;
            }

            this.dbContext.Locations.Add(location);
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLocation(string name)
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

            await this.dbContext.SaveChangesAsync();
            return true;
        }

        private void ChangeLocationOfOrder(ICollection<Order> orders)
        {
            foreach (var order in orders)
            {
                order.ReturnLocationId = this.GetDefaultLocation().Id;
            }
        }

        private void ChangeLocationOfCar(ICollection<Car> cars)
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
            var location = this.dbContext.
                Locations.
                FirstOrDefault(x => x.Name == name);

            if(location is null)
            {
                return 0;
            }

            return location.Id;
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
