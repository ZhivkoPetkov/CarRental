using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Data;
using Microsoft.AspNetCore.SignalR;

namespace CarRental.Web.Hubs
{
    public class NotifyHub : Hub
    {
        private readonly CarRentalDbContext dbContext;

        public NotifyHub(CarRentalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task GetHello()
        {
            while (true)
            {
                var lastOrder = dbContext.Orders.First();
                var timeNow = DateTime.UtcNow;

                if ((timeNow - lastOrder.CreatedOn).TotalSeconds <= 2)
                {
                    await this.Clients.All.SendAsync("NotifyOrders", $"{lastOrder.Car.Model} was just ordered");
                }
            }
        }
    }
}
