using CarRental.Data;
using CarRental.Models;
using CarRental.Services.Contracts;
using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Services
{
    public class CarsService : ICarsService
    {
        private readonly CarRentalDbContext dbContext;
        private readonly Cloudinary cloudinary;

        public CarsService(CarRentalDbContext dbContext, Cloudinary cloudinary)
        {
            this.dbContext = dbContext;
            this.cloudinary = cloudinary;
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
