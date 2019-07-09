using CarRental.Models;
using CarRental.Models.Enums;

namespace CarRental.DTOs.Vouchers
{
    public class VoucherDto
    {
        public int Id { get; set; }
        public string VoucherCode { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int Discount { get; set; }

        public VoucherStatus Status { get; set; }
    }
}
