using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.InputModels.Orders
{
    public class OrderInputViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime PickUp { get; set; }
        [Required]
        public DateTime Return { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Required]
        public string PickUpPlace { get; set; }
        [Required]
        public string ReturnPlace { get; set; }

        public string DiscountCode { get; set; }
    }
}
