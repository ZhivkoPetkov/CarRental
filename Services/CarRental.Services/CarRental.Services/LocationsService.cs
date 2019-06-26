using CarRental.Data;
using CarRental.Models;
using CarRental.Services.Contracts;
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

        public ICollection<string> GetAllLocationNames()
        {
            return this.dbContext.Locations.
                Select(x => x.Name).
                ToList();
        }

        public string GetIdByName(string name)
        {
            return this.dbContext.
                Locations.
                FirstOrDefault(x => x.Name == name).Id;
        }
    }
}
