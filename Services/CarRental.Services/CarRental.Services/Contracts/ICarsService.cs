using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Services.Contracts
{
    public interface ICarsService
    {
        bool AddCar(Car car);
    }
}
