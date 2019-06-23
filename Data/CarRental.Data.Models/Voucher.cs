using CarRental.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Data.Models
{
   public class Voucher
    {
        public Voucher()
        {
            this.VoucherCode = Guid.NewGuid().ToString();
            this.Status = VoucherStatus.Active;
        }

        public int Id { get; set; }

        public string VoucherCode { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser User { get; set; }

        public int Discount { get; set; }

        public VoucherStatus Status { get; set; }
    }
}
