using CarRental.Data;
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

        public OrdersService(CarRentalDbContext dbContext, UserManager<ApplicationUser> userManager, IUsersService usersService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.usersService = usersService;
        }

        public bool MakeOrder(string customer, int carId, string startLocation, string returnLocation, decimal price, 
                                            DateTime startRent, DateTime endRent)
        {
            var userId = this.usersService.GetUserIdByEmail(customer);
            var order = new Order
            {
                ApplicationUserId = userId,
                CarId = carId,
                RentEnd = endRent,
                RentStart = startRent,
                Price = price,
                Status = Models.Enums.OrderStatus.Active
            };

            this.dbContext.Orders.Add(order);
            this.dbContext.SaveChanges();

            return true;
        }
    }
}
