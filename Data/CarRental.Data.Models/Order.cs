using CarRental.Data.Models.Enums;
using System;

namespace CarRental.Data.Models
{
    public class Order
    {
        public string Id { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser User { get; set; }

        public decimal Price { get; set; }

        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public OrderStatus Status { get; set; }

        public int? VoucherId { get; set; }

        public Voucher Voucher { get; set; }
    }
}
