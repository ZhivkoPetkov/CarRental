using CarRental.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Services.Contracts
{
   public interface IOrdersService
    {
        bool MakeOrder(string userId, int carId, string startLocation, string returnLocation, 
                                    decimal price, DateTime startRent, DateTime endRent, string voucherCode);

        ICollection<OrderDto> GetAllOrdersForUser(string email);

        ICollection<OrderDto> All();
        OrderDto GetOrderById(string id);

        bool Delete(string id);
        bool Cancel(string id);
        bool Finish(string id);

        bool IsValidReviewRequest(string orderId, string customerEmail);
    }
}
