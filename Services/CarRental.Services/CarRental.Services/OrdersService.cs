using AutoMapper;
using CarRental.Data;
using CarRental.DTOs.Orders;
using CarRental.Models;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRental.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly CarRentalDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly IMapper mapper;
        private readonly ILocationsService locationsService;
        private readonly ICarsService carsService;

        public OrdersService(CarRentalDbContext dbContext, UserManager<ApplicationUser> userManager, 
                        IUsersService usersService, IMapper mapper, ILocationsService locationsService, ICarsService carsService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.usersService = usersService;
            this.mapper = mapper;
            this.locationsService = locationsService;
            this.carsService = carsService;
        }

        public ICollection<OrderDto> All()
        {
            var orders = this.dbContext.Orders.ToList();

            return mapper.Map<List<OrderDto>>(orders);
        }

        public bool Cancel(string id)
        {
            var order = this.dbContext.Orders.Find(id);
            if (order is null)
            {
                return false;
            }
            order.Status = Models.Enums.OrderStatus.Canceled;

            this.CancelRentDays(order);

            this.dbContext.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var order = this.dbContext.Orders.Find(id);
            if (order is null)
            {
                return false;
            }
            this.dbContext.Orders.Remove(order);
            this.dbContext.SaveChanges();

            return true;
        }

        public bool Finish(string id)
        {
            var order = this.dbContext.Orders.Find(id);

            if (order is null || order.RentEnd > DateTime.UtcNow.Date)
            {
                return false;
            }

            order.Status = Models.Enums.OrderStatus.Finished;
            this.carsService.ChangeLocation(order.CarId, order.ReturnLocationId);

            this.dbContext.SaveChanges();

            return true;
        }

        public ICollection<OrderDto> GetAllOrdersForUser(string email)
        {
            var user = this.usersService.GetUserByEmail(email);
          
            var orders = user.Orders.ToList();

            return mapper.Map <List<OrderDto>> (orders);

        }

        public OrderDto GetOrderById(string id)
        {
            var order = this.dbContext.Orders.Find(id);

            return mapper.Map<OrderDto>(order);
        }

        public bool IsValidReviewRequest(string orderId, string customerEmail)
        {
            var order = this.dbContext.Orders.Find(orderId);

            return order.User.Email == customerEmail;
        }

        public bool MakeOrder(string customer, int carId, string startLocation, string returnLocation, decimal price, 
                                            DateTime startRent, DateTime endRent)
        {
            var userId = this.usersService.GetUserIdByEmail(customer);
            var pikcupLocationId = this.locationsService.GetIdByName(startLocation);
            var returnLocationId = this.locationsService.GetIdByName(returnLocation);
            var order = new Order
            {
                ApplicationUserId = userId,
                CarId = carId,
                RentEnd = endRent,
                RentStart = startRent,
                Price = price,
                PickUpLocationId = pikcupLocationId,
                ReturnLocationId = returnLocationId,
                Status = Models.Enums.OrderStatus.Active
            };

            this.dbContext.Orders.Add(order);
            this.dbContext.SaveChanges();

            var rentCar = this.carsService.RentCar(startRent, endRent, carId);

            return true;
        }

       
        private void CancelRentDays(Order order)
        {
            
            for (var dt = order.RentStart; dt <= order.RentEnd; dt = dt.AddDays(1))
            {
                var rentday = order.Car.RentDays.FirstOrDefault(x => x.RentDate.Date == dt);
                dbContext.CarRentDays.Remove(rentday);
            }
        }
    }
}
