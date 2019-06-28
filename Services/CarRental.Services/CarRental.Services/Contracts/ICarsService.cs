using CarRental.Models;
using System;
using System.Collections.Generic;

namespace CarRental.Services.Contracts
{
    public interface ICarsService
    {
        bool AddCar(Car car);

        ICollection<Car> GetAvailableCars(DateTime start, DateTime end);
    }
}
