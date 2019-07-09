using CarRental.DTOs.Vouchers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Services.Contracts
{
   public interface IVouchersService
    {
        bool CreateForUser(string username);
        bool CreateForUserCustom(string username, int discount);
        bool DeleteVoucher(int id);
        ICollection<VoucherDto> GetAllVouchers();
        ICollection<VoucherDto> GetAllForUser(string username);
        ICollection<VoucherDto> GetAllActiveForUser(string username);
        bool UseVoucher(string voucherCode);
        int GetDiscountForCode(string voucherCode);
    }
}
