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

        CarDetailsDto FindCar(int id);
    }
}
