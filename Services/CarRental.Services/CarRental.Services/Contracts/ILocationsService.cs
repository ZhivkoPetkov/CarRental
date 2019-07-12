using CarRental.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Services.Contracts
{
    public interface ILocationsService
    {
        Task<bool> CreateLocation(Location location);

        Task<bool> DeleteLocation(string name);
        ICollection<string> GetAllLocationNames();

        int GetIdByName(string name);
    }
}
