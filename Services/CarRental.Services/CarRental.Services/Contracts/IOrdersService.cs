using CarRental.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Services.Contracts
{
   public interface IOrdersService
    {
        bool MakeOrder(string userId, int carId, string startLocation, string returnLocation, 
                                    decimal price, DateTime startRent, DateTime endRent);

        ICollection<OrderDto> GetAllOrdersForUser(string email);
    }
}
