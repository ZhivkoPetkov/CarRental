using CarRental.DTOs.Vouchers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Services.Contracts
{
   public interface IVouchersService
    {
        Task<bool> CreateForUser(string email);
        Task<bool> CreateForUserCustom(string email, int discount);
        Task<bool> DeleteVoucher(int id);
        ICollection<VoucherDto> GetAllVouchers();
        ICollection<VoucherDto> GetAllForUser(string username);
        ICollection<VoucherDto> GetAllActiveForUser(string username);
        Task<bool> UseVoucher(string voucherCode);
        Task<int> GetDiscountForCode(string voucherCode);
    }
}
