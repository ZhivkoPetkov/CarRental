using CarRental.Models;

namespace CarRental.Services.Contracts
{
    public interface ILocationsService
    {
        bool CreateLocation(Location location);
    }
}
