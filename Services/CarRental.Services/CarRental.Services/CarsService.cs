using AutoMapper;
using CarRental.Common;
using CarRental.Data;
using CarRental.DTOs.Cars;
using CarRental.DTOs.Reviews;
using CarRental.Models;
using CarRental.Services.Contracts;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<bool> AddCar(Car car)
        {         
            var result = this.dbContext.Cars.Add(car);
            if (result.State != EntityState.Added)
            {
                return false;
            }
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeLocation(int id, int returnLocationId)
        {
            var car = this.dbContext.Cars.Find(id);
            if (car is null)
            {
                return false;
            }
            car.LocationId = returnLocationId;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditCar(Car car)
        {
            var oldCar = this.dbContext.Cars.Find(car.Id);
            if (oldCar is null)
            {
                return false;
            }
            oldCar.Model = car.Model;
            oldCar.Description = car.Description;
            oldCar.Year = car.Year;
            oldCar.PricePerDay = car.PricePerDay;
            oldCar.Image = car.Image;
            var result = await this.dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteCar(int id)
        {
            var car = this.dbContext.Cars.Find(id);
            if (car is null)
            {
                return false;
            }
            car.inUse = false;
            var result = await this.dbContext.SaveChangesAsync();
            return true;
        }

        public CarDetailsDto FindCar(int id)
        {
                var car = this.dbContext.Cars.Find(id);
                if (car is null || !car.inUse)
                {
                    return null;
                }
                var result = this.mapper.Map<CarDetailsDto>(car);
                result.Reviews = this.mapper.Map<List<ReviewDto>>(car.Reviews);
                return result;
        }

        public ICollection<ListCarDto> GetAllCars(string orderby)
        {
            var cars = this.dbContext.
                Cars.
                Where(x => x.inUse == true).
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
                    RentDays = x.RentDays,
                    Reviews = this.mapper.Map<List<ReviewDto>>(x.Reviews)
                }).
                ToList();

            if (orderby == GlobalConstants.OrderCarsByRentsAscending)
            {
                return cars.OrderBy(x => x.RentDays.Count).ToList();
            }
            else if (orderby == GlobalConstants.OrderCarsByRentsDescending)
            {
                return cars.OrderByDescending(x => x.RentDays.Count).ToList();
            }
            else if (orderby == GlobalConstants.OrderCarsByRatingDescending)
            {
                return cars.OrderByDescending(x => x.Reviews.Select(p => p.Rating).DefaultIfEmpty(0).Average()).ToList();
            }
            else if (orderby == GlobalConstants.OrderCarsByLastAdded)
            {
                cars.Reverse();
            }
            else if (orderby == GlobalConstants.OrderCarsByPriceAscending)
            {
                return cars.OrderBy(x => x.PricePerDay).ToList();
            }
            else if (orderby == GlobalConstants.OrderCarsByPriceDescending)
            {
                return cars.OrderByDescending(x => x.PricePerDay).ToList();
            }

            return cars;
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
                Include(x => x.Location).
                Where(x => x.RentDays.Any(d => dates.Contains(d.RentDate)) == false).
                Where(l => l.Location.Name == location).
                Where(x => x.inUse == true).
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

        public CarDetailsDto FindCarForEdit(int id)
        {
            var car = this.dbContext.Cars.Find(id);
            if (!car.inUse)
            {
                return null;
            }
            var result = this.mapper.Map<CarDetailsDto>(car);
            return result;
        }

        public async Task<bool> RentCar(DateTime start, DateTime end, int cardId)
        {
            var dates = new List<CarRentDays>();
            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dates.Add(new CarRentDays
                {
                    CarId = cardId,
                    RentDate = dt
                });
            }

            this.dbContext.CarRentDays.AddRange(dates);
            await this.dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> IsAlreadyRented(DateTime start, DateTime end, int cardId)
        {
            var dates = new List<DateTime>();
            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            foreach (var date in dates)
            {
                if (dbContext.CarRentDays.Any(x => x.CarId == cardId && x.RentDate == date))
                {
                    return true;
                }
            }            
            return false;
        }
    }
}
