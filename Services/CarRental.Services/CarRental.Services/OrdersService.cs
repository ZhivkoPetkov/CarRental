using AutoMapper;
using CarRental.Data;
using CarRental.DTOs.Orders;
using CarRental.Models;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly IVouchersService vouchersService;

        public OrdersService(CarRentalDbContext dbContext, UserManager<ApplicationUser> userManager,
                        IUsersService usersService, IMapper mapper,
                        ILocationsService locationsService, ICarsService carsService, IVouchersService vouchersService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.usersService = usersService;
            this.mapper = mapper;
            this.locationsService = locationsService;
            this.carsService = carsService;
            this.vouchersService = vouchersService;
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
            if (order.Status != Models.Enums.OrderStatus.Canceled)
            {
                this.CancelRentDays(order);
            }

            this.dbContext.Orders.Remove(order);
            this.dbContext.SaveChanges();

            return true;
        }

        public bool DeleteReviewFromOrder(int reviewId)
        {
            var order = this.dbContext.
                Orders.
                Where(x => x.ReviewId == reviewId).
                FirstOrDefault();

            if (order is null)
            {
                return false;
            }
            order.ReviewId = null;
            this.dbContext.SaveChanges();

            return true;
        }

        public bool EditOrder(string id, string firstName, string lastName, string email, decimal price)
        {
            var order = this.dbContext.Orders.Find(id);
            if (order is null)
            {
                return false;
            }

            order.User.FirstName = firstName;
            order.User.LastName = lastName;
            order.User.Email = email;
            order.Price = price;
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

            return mapper.Map<List<OrderDto>>(orders);

        }

        public OrderDto GetOrderById(string id)
        {
            var order = this.dbContext.Orders.Find(id);

            return mapper.Map<OrderDto>(order);
        }

        public bool IsValidReviewRequest(string orderId, string customerEmail)
        {
            var order = this.dbContext.Orders.Find(orderId);

            return order.User.Email.ToLower() == customerEmail;
        }

        public bool MakeOrder(string customer, int carId, string startLocation, string returnLocation, decimal price,
                                            DateTime startRent, DateTime endRent, string voucherCode)
        {
            var userId = this.usersService.GetUserIdByEmail(customer);
            var pikcupLocationId = this.locationsService.GetIdByName(startLocation);
            var returnLocationId = this.locationsService.GetIdByName(returnLocation);

            if (voucherCode != "none")
            {
                var result = this.vouchersService.UseVoucher(voucherCode);
                if (!result)
                {
                    return false;
                }
            }

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
