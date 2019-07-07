using CarRental.Models;
using CarRental.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.DTOs.Vouchers
{
    public class VoucherDto
    {
        public string VoucherCode { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int Discount { get; set; }

        public VoucherStatus Status { get; set; }
    }
}
