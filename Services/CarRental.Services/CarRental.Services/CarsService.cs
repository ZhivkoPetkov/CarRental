using AutoMapper;
using CarRental.Data;
using CarRental.DTOs.Cars;
using CarRental.Models;
using CarRental.Services.Contracts;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Services
{
    public class CarsService : ICarsService
    {
        private readonly CarRentalDbContext dbContext;
        private readonly Cloudinary cloudinary;
        private readonly IMapper mapper;

        public CarsService(CarRentalDbContext dbContext, Cloudinary cloudinary, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.cloudinary = cloudinary;
            this.mapper = mapper;
        }
        public bool AddCar(Car car)
        {         
            car.IsRented = false;
            this.dbContext.Cars.Add(car);
            this.dbContext.SaveChanges();
            return true;
        }

        public ICollection<ListCarDto> GetAvailableCars(DateTime start, DateTime end, string location)
        {
            var dates = new List<DateTime>();
            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            var cars = this.dbContext.
                Cars.
                Where(x => x.RentDays.Any(d => dates.Contains(d.RentDate)) == false).
                Where(l => l.Location.Name == location).
                Include(x => x.Location).
                Select(x => new ListCarDto
                {
                    Id = x.Id,
                    Image = x.Image,
                    Description = x.Description,
                    GearType = x.GearType,
                    Location = x.Location.Name,
                    PricePerDay = x.PricePerDay,
                    Model = x.Model,
                    Year = x.Year,
                    Days = dates.Count(),
                    StartRent = start,
                    End = end
                }).
                ToList();
            return cars;
        }
    }
}
