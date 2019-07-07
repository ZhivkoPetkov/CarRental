using CarRental.DTOs.Vouchers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Services.Contracts
{
   public interface IVouchersService
    {
        bool CreateForUser(string username);
        ICollection<VoucherDto> GetAllForUser(string username);
    }
}
