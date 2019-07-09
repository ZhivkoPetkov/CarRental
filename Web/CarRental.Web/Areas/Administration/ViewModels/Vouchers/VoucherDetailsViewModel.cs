using CarRental.Models.Enums;

namespace CarRental.Web.Areas.Administration.ViewModels.Vouchers
{
    public class VoucherDetailsViewModel
    {
        public int Id { get; set; }

        public string VoucherCode { get; set; }
      
        public string User { get; set; }

        public int Discount { get; set; }

        public VoucherStatus Status { get; set; }
    }
}
