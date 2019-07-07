using CarRental.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Voucher
    {
        public Voucher()
        {
            this.VoucherCode = Guid.NewGuid().ToString();
            this.Status = VoucherStatus.Active;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string VoucherCode { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        [Required]
        public int Discount { get; set; }
        [Required]
        public VoucherStatus Status { get; set; }
    }
}
