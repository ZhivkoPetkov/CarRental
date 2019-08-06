using AutoMapper;
using CarRental.Common;
using CarRental.Data;
using CarRental.DTOs.Vouchers;
using CarRental.Models;
using CarRental.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace CarRental.Services
{
    public class VouchersService : IVouchersService
    {
        private readonly CarRentalDbContext dbCotenxt;
        private readonly IUsersService usersService;
        private readonly IMapper mapper;

        public VouchersService(CarRentalDbContext dbCotenxt, IUsersService usersService, IMapper mapper)
        {
            this.dbCotenxt = dbCotenxt;
            this.usersService = usersService;
            this.mapper = mapper;
        }

        public async Task<bool> CreateForUser(string email)
        {
            var userId = this.usersService.GetUserIdByEmail(email);
            if (userId is null)
            {
                return false;
            }
            var voucher = new Voucher
            {
                ApplicationUserId = userId,
                Status = Models.Enums.VoucherStatus.Active,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = this.GenerateDiscount()
            };
            this.dbCotenxt.Vouchers.Add(voucher);
            await this.dbCotenxt.SaveChangesAsync();
            return true;
        }

        public bool CreateForUserCustom(string email, int discount)
        {
            var userId = this.usersService.GetUserIdByEmail(email);
            if (userId is null)
            {
                return false;
            }
            var voucher = new Voucher
            {
                ApplicationUserId = userId,
                Status = Models.Enums.VoucherStatus.Active,
                VoucherCode = Guid.NewGuid().ToString(),
                Discount = discount
            };
            this.dbCotenxt.Vouchers.Add(voucher);
            this.dbCotenxt.SaveChanges();
            return true;
        }

        public bool DeleteVoucher(int id)
        {
            var voucher = this.dbCotenxt.
                             Vouchers.Find(id);

            if (voucher is null)
            {
                return false;
            }

            this.dbCotenxt.Vouchers.Remove(voucher);
            this.dbCotenxt.SaveChanges();
            return true;
        }

        public ICollection<VoucherDto> GetAllActiveForUser(string username)
        {
            var userId = this.usersService.GetUserIdByEmail(username);

            var vouchers = this.dbCotenxt.Vouchers.Where(x => x.ApplicationUserId == userId && x.Status == Models.Enums.VoucherStatus.Active);

            return this.mapper.Map<List<VoucherDto>>(vouchers);
        }

        public ICollection<VoucherDto> GetAllForUser(string username)
        {
            var userId = this.usersService.GetUserIdByEmail(username);

            var vouchers = this.dbCotenxt.Vouchers.Where(x => x.ApplicationUserId == userId);

            return this.mapper.Map<List<VoucherDto>>(vouchers);
        }

        public ICollection<VoucherDto> GetAllVouchers()
        {
            var vouchers = this.dbCotenxt.
                             Vouchers.ToList();

            return this.mapper.Map<List<VoucherDto>>(vouchers);
        }

        public async Task<int> GetDiscountForCode(string voucherCode)
        {
            if (String.IsNullOrEmpty(voucherCode) ||voucherCode == GlobalConstants.DefaultVoucherCode)
            {
                return 0;
            }

            var voucher = await this.dbCotenxt.Vouchers.AsAsyncEnumerable()
                .FirstOrDefault(x => x.VoucherCode == voucherCode);

            return voucher == null ? 0 : voucher.Discount;
        }

        public async Task<bool> UseVoucher(string voucherCode)
        {
            var voucher = this.dbCotenxt.Vouchers.
                                Where(x => x.VoucherCode == voucherCode).
                                FirstOrDefault();

            if (voucher is null)
            {
                return false;
            }

            voucher.Status = Models.Enums.VoucherStatus.Used;
            await this.dbCotenxt.SaveChangesAsync();
            return true;
        }

        private int GenerateDiscount()
        {
            var random = new Random();
            var value = random.Next(GlobalConstants.MinimumVoucherDiscount, GlobalConstants.MaximumVoucherDiscount);
            return value;
        }
    }
}
