using CarRental.Models.Enums;

namespace CarRental.Web.ViewModels.Vouchers
{
    public class VoucherViewModel
    {
        public string VoucherCode { get; set; }

        public int Discount { get; set; }

        public VoucherStatus Status { get; set; }
    }
}
