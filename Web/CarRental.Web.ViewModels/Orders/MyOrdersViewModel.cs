using System;
using CarRental.Models.Enums;

namespace CarRental.Web.ViewModels.Orders
{
    public class MyOrdersViewModel
    {
        public string Id { get; set; }
        public string CarModel { get; set; }
        public string PickUpLocation { get; set; }
        public string ReturnLocation { get; set; }
        public decimal Price { get; set; }

        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public OrderStatus Status { get; set; }

        public int ReviewId { get; set; }

    }
}
