using CarRental.Data;
using CarRental.Models;
using CarRental.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Services
{
    public class CarsService : ICarsService
    {
        private readonly CarRentalDbContext dbContext;

        public CarsService(CarRentalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool AddCar(Car car)
        {
            car.IsRented = false;
            this.dbContext.Cars.Add(car);
            this.dbContext.SaveChanges();
            return true;
        }
    }
}
