using CarRental.DTOs.Cars;
using CarRental.DTOs.Locations;
using CarRental.DTOs.Reviews;
using CarRental.DTOs.Users;
using CarRental.Models.Enums;
using System;

namespace CarRental.DTOs.Orders
{
    public class OrderDto
    {
        public string Id { get; set; }

        public CarDetailsDto Car { get; set; }

        public string ApplicationUserId { get; set; }

        public UserDto User { get; set; }

        public decimal Price { get; set; }

        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public OrderStatus Status { get; set; }

        public int PickUpLocationId { get; set; }

        public LocationDto PickUpLocation { get; set; }

        public int ReturnLocationId { get; set; }

        public LocationDto ReturnLocation { get; set; }

        public int? ReviewId { get; set; }
        public ReviewDto Review { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
