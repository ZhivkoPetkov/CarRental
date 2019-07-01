using CarRental.DTOs.Cars;
using CarRental.Models;
using System;
using System.Collections.Generic;

namespace CarRental.Services.Contracts
{
    public interface ICarsService
    {
        bool AddCar(Car car);

        ICollection<ListCarDto> GetAvailableCars(DateTime start, DateTime end, string location);
        ICollection<ListCarDto> GetAllCars(string orderBy);

        bool RentCar(DateTime start, DateTime end, int cardId);

        CarDetailsDto FindCar(int id);

        bool ChangeLocation(int id, int returnLocationId);
    }
}
