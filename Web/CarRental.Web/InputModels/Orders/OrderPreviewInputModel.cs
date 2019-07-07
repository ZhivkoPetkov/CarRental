using CarRental.Web.ViewModels.Vouchers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.InputModels.Orders
{
    public class OrderPreviewInputModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public decimal PricePerDay { get; set; }
        [Required]
        public DateTime RentStart { get; set; }
        [Required]
        public DateTime RentEnd { get; set; }
        [Required]
        public int Days { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string PickUpPlace { get; set; }
        [Required]
        public string ReturnPlace { get; set; }

        public string DiscountCode { get; set; } = "none";

        public int DiscountPercent { get; set; }

        public decimal PriceWithoutDiscount => this.PricePerDay * this.Days;

        public decimal DiscountSum => ((decimal)this.DiscountPercent / 100) * this.PriceWithoutDiscount;

        public decimal TotalPrice => this.PriceWithoutDiscount - this.DiscountSum;

        public ICollection<VoucherViewModel> Vouchers { get; set; } = new HashSet<VoucherViewModel>();
    }
}
