using CarRental.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Services.Contracts
{
   public interface IOrdersService
    {
        Task<bool> MakeOrder(string email, int carId, string startLocation, string returnLocation, 
                                    decimal price, DateTime startRent, DateTime endRent, string voucherCode);

        ICollection<OrderDto> GetAllOrdersForUser(string email);
        Task<bool> DeleteReviewFromOrder(int reviewId);
        ICollection<OrderDto> All();
        OrderDto GetOrderById(string id);
        bool UserFinishedOrders(string name);
        Task<bool> Delete(string id);
        Task<bool> Cancel(string id);
        Task<bool> Finish(string id);
        Task<bool> EditOrder(string id, string firstName, string lastName, string email, decimal price);
        Task<bool> IsValidReviewRequest(string orderId, string customerEmail);
    }
}
