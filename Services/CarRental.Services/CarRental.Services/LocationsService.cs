using CarRental.Data;
using CarRental.Models;
using CarRental.Services.Contracts;

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
            this.dbContext.Locations.Add(location);
            this.dbContext.SaveChanges();
            return true;
        }
    }
}
