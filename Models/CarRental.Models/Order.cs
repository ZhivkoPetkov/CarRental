using CarRental.Models;
using CarRental.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Order
    {

        public Order()
        {
            Id = Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }

        [Key]
        public string Id { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public decimal Price { get; set; }

        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public OrderStatus Status { get; set; }

        public int? VoucherId { get; set; }

        public virtual Voucher Voucher { get; set; }

        public int PickUpLocationId { get; set; }

        public virtual Location PickUpLocation { get; set; }

        public int ReturnLocationId { get; set; }

        public virtual Location ReturnLocation { get; set; }
    }
}
