using AutoMapper;
using CarRental.Data;
using CarRental.DTOs.Orders;
using CarRental.Models;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Identity;
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

        public ICollection<OrderDto> GetAllOrdersForUser(string email)
        {
            var user = this.usersService.GetUserByEmail(email);
          
            var orders = user.Orders.ToList();

            return mapper.Map <List<OrderDto>> (orders);

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
    }
}
