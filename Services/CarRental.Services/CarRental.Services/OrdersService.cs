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

        private readonly IUsersService usersService;
        private readonly IMapper mapper;
        private readonly ILocationsService locationsService;
        private readonly ICarsService carsService;
        private readonly IVouchersService vouchersService;

        public OrdersService(CarRentalDbContext dbContext, IUsersService usersService, IMapper mapper,
                        ILocationsService locationsService, ICarsService carsService, IVouchersService vouchersService)
        {
            this.dbContext = dbContext;
            this.usersService = usersService;
            this.mapper = mapper;
            this.locationsService = locationsService;
            this.carsService = carsService;
            this.vouchersService = vouchersService;
        }

        public ICollection<OrderDto> All()
        {
            var orders = this.dbContext.Orders.OrderByDescending(x => x.CreatedOn).
                                                ToList();

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

            if (order is null)
            {
                return false;
            }

            var changedLocation = await this.carsService.ChangeLocation(order.CarId, order.ReturnLocationId);

            if (!changedLocation)
            {
                return false;
            }

            order.Status = Models.Enums.OrderStatus.Finished;
            this.dbContext.SaveChanges();

            return true;
        }

        public ICollection<OrderDto> GetAllOrdersForUser(string email)
        {
            var user = this.usersService.GetUserByEmail(email);

            if (user is null)
            {
                return new List<OrderDto>();
            }

            var orders = user.Orders.OrderByDescending(x => x.CreatedOn).
                                        ToList();

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

            if (order is null)
            {
                return false;
            }

            return order.User.Email.ToLower() == customerEmail;
        }

        public async Task<bool> MakeOrder(string email, int carId, string startLocation, string returnLocation, decimal price,
                                            DateTime startRent, DateTime endRent, string voucherCode)
        {
            var userId = this.usersService.GetUserIdByEmail(email);
            var pickupLocationId = this.locationsService.GetIdByName(startLocation);
            var returnLocationId = this.locationsService.GetIdByName(returnLocation);

            if (userId is null || pickupLocationId == 0 || returnLocationId == 0)
            {
                return false;
            }

            if (carsService.IsAlreadyRented(startRent,endRent, carId).GetAwaiter().GetResult())
            {
                return false;
            }

            //If the voucher is different from none, discount will be generated and the voucher will be with status Used
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
                PickUpLocationId = pickupLocationId,
                ReturnLocationId = returnLocationId,
                Status = Models.Enums.OrderStatus.Active
            };

            var rentTheCar = await this.carsService.RentCar(startRent, endRent, carId);
            if (!rentTheCar)
            {
                return false;
            }

            this.dbContext.Orders.Add(order);
            this.dbContext.SaveChanges();

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
                var rentDay = order.Car.RentDays.FirstOrDefault(x => x.RentDate.Date == dt);
                dbContext.CarRentDays.Remove(rentDay);
            }
        }
    }
}
