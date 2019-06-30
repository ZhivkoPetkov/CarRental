using CarRental.Models;
using CarRental.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.DTOs.Orders
{
   public class OrderDto
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

        public int PickUpLocationId { get; set; }

        public virtual Location PickUpLocation { get; set; }

        public int ReturnLocationId { get; set; }

        public virtual Location ReturnLocation { get; set; }

    }
}
