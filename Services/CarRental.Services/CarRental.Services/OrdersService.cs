using AutoMapper;
using CarRental.Data;
using CarRental.DTOs.Orders;
using CarRental.Models;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<bool> Cancel(string id)
        {
            var order = this.dbContext.Orders.Find(id);
            if (order is null)
            {
                return false;
            }
            order.Status = Models.Enums.OrderStatus.Canceled;

            this.CancelRentDays(order);

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(string id)
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
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteReviewFromOrder(int reviewId)
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
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditOrder(string id, string firstName, string lastName, string email, decimal price)
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
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Finish(string id)
        {
            var order = this.dbContext.Orders.Find(id);

            if (order is null || order.RentEnd > DateTime.UtcNow.Date)
            {
                return false;
            }

            order.Status = Models.Enums.OrderStatus.Finished;
            this.carsService.ChangeLocation(order.CarId, order.ReturnLocationId);

            await this.dbContext.SaveChangesAsync();

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

        public async Task<bool> IsValidReviewRequest(string orderId, string customerEmail)
        {
            var order = await this.dbContext.Orders.FindAsync(orderId);

            return order.User.Email.ToLower() == customerEmail;
        }

        public async Task<bool> MakeOrder(string customer, int carId, string startLocation, string returnLocation, decimal price,
                                            DateTime startRent, DateTime endRent, string voucherCode)
        {
            var userId = this.usersService.GetUserIdByEmail(customer);
            var pikcupLocationId = this.locationsService.GetIdByName(startLocation);
            var returnLocationId = this.locationsService.GetIdByName(returnLocation);

            if (voucherCode != "none")
            {
                var result = this.vouchersService.UseVoucher(voucherCode).GetAwaiter().GetResult();
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
            await this.dbContext.SaveChangesAsync();
            var rentCar = this.carsService.RentCar(startRent, endRent, carId);
            return true;
        }

        public bool UserFinishedOrders(string name)
        {
            return this.dbContext.Orders.
                Any(x => x.User.UserName == name && x.Status == Models.Enums.OrderStatus.Finished && x.ReviewId == null);
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
