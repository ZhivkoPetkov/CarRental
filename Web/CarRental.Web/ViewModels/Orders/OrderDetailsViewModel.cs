using CarRental.Models.Enums;
using System;

namespace CarRental.Web.ViewModels.Orders
{
    public class OrderDetailsViewModel
    {
        public string Id { get; set; }

        public string CarModel { get; set; }

        public GearType CarGearType { get; set; }

        public int CarYear { get; set; }

        public string CarImage { get; set; }

        public string Email { get; set; }
        public string CarDescription { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public decimal Price { get; set; }

        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public OrderStatus Status { get; set; }

        public string PickUpLocation  { get; set; }

        public string ReturnLocation { get; set; }

        public int? ReviewId { get; set; }
    }
}
