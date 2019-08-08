using CarRental.DTOs.Cars;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Services.Contracts
{
    public interface ICarsService
    {
        Task<bool> AddCar(Car car);

        Task<bool> EditCar(Car car);
        Task<bool> DeleteCar(int id);

        ICollection<ListCarDto> GetAvailableCars(DateTime start, DateTime end, string location);
        ICollection<ListCarDto> GetAllCars(string orderBy);

        Task<bool> RentCar(DateTime start, DateTime end, int cardId);

        Task<CarDetailsDto> FindCar(int id);
        Task<CarDetailsDto> FindCarForEdit(int id);

        Task<string> GetCarModelById(int id);

        Task<bool> IsAlreadyRented(DateTime start, DateTime end, int cardId);

        Task<bool> ChangeLocation(int id, int returnLocationId);
    }
}
