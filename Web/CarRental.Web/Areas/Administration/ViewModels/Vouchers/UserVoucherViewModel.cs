using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.Areas.Administration.ViewModels.Vouchers
{
    public class UserVoucherViewModel
    {
        public string Email { get; set; }

        public decimal MoneySpent { get; set; }

        public int Rents { get; set; }

        [Required]
        [Range(1,100, ErrorMessage = "The discount percent should be between {1}% and {2}%")]
        public int Discount { get; set; }
    }
}
