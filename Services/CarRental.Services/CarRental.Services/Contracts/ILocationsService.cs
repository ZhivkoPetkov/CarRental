using CarRental.Models;
using System.Collections.Generic;

namespace CarRental.Services.Contracts
{
    public interface ILocationsService
    {
        bool CreateLocation(Location location);

        ICollection<string> GetAllLocationNames();

        int GetIdByName(string name);
    }
}
