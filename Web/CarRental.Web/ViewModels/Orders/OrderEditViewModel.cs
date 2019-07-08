using CarRental.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.ViewModels.Orders
{
    public class OrderEditViewModel
    {
        public string Id { get; set; }

        public string CarModel { get; set; }

        public int CarYear { get; set; }

        public string CarImage { get; set; }
        [Required]
        public string Email { get; set; }

        public string CarDescription { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public decimal Price { get; set; }

        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public OrderStatus Status { get; set; }

        public string PickUpLocation { get; set; }

        public string ReturnLocation { get; set; }

    }
}
