using AutoMapper;
using CarRental.Data;
using CarRental.DTOs.Vouchers;
using CarRental.Models;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public bool CreateForUser(string username)
        {
            var userId = this.usersService.GetUserIdByEmail(username);
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
            this.dbCotenxt.SaveChanges();
            return true;
        }

        public ICollection<VoucherDto> GetAllForUser(string username)
        {
            var userId = this.usersService.GetUserIdByEmail(username);

            var vouchers = this.dbCotenxt.Vouchers.Where(x => x.ApplicationUserId == userId);

            return this.mapper.Map<List<VoucherDto>>(vouchers);
        }

        public bool UseVoucher(string voucherCode)
        {
            var voucher = this.dbCotenxt.Vouchers.
                Where(x => x.VoucherCode == voucherCode).
                FirstOrDefault();

            if (voucher is null)
            {
                return false;
            }

            voucher.Status = Models.Enums.VoucherStatus.Used;
            this.dbCotenxt.SaveChanges();
            return true;
        }

        private int GenerateDiscount()
        {
            var random = new Random();
            var value = random.Next(0, 5);
            return value;
        }
    }
}
