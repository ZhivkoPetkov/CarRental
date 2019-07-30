using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarRental.Data;
using CarRental.Models;
using CarRental.Models.Enums;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarRental.Services.Tests
{
    public class VouchersServiceTests : BaseServiceTests
    {

        [Fact]
        public void CreateForUserShould_CreateVoucherForUser()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CreateVoucher")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                Returns(user.Id);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            Assert.False(dbContext.Vouchers.Any(x => x.ApplicationUserId == user.Id));

            vouchersService.CreateForUser(user.Email);

            Assert.True(dbContext.Vouchers.Any(x => x.ApplicationUserId == user.Id));
        }


        [Fact]
        public void CreateCustomForUserShould_CreateVoucherForUser()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CustomVoucher")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                Returns(user.Id);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            Assert.False(dbContext.Vouchers.Any(x => x.ApplicationUserId == user.Id));

            var random = new Random();
            var discount = random.Next(1, 100);
            vouchersService.CreateForUserCustom(user.Email,discount);

            Assert.True(dbContext.Vouchers.Any(x => x.ApplicationUserId == user.Id && x.Discount == discount));
        }


        [Fact]
        public void CreateForUserShould_ReturnFalseIfInvalidUser()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CustomVoucher")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                Returns(user.Id);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            var result = vouchersService.CreateForUser(Guid.NewGuid().ToString()).GetAwaiter().GetResult();

            Assert.False(result);
        }


        [Fact]
        public void DeleteVoucherShould_DeleteValidVoucher()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_CustomVoucher")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                Returns(user.Id);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            var result = vouchersService.CreateForUser(user.Email);
            Assert.True(dbContext.Vouchers.Any(x => x.ApplicationUserId == user.Id));

            var voucherId = dbContext.Vouchers.FirstOrDefault().Id;
            vouchersService.DeleteVoucher(voucherId);

            Assert.False(dbContext.Vouchers.Any(x => x.ApplicationUserId == user.Id));
        }

        [Fact]
        public void DeleteVoucherShould_ReturnFalseIfInvalidVoucherId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DeleteVoucherInvalid")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                Returns(user.Id);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            var result = vouchersService.DeleteVoucher(-1);

            Assert.False(result);
        }

        [Fact]
        public void GetAllActiveForUserShould_ReturnOnlyActiveVouchersForUser()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_UserActiveVouchers")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                Returns(user.Id);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            for (int i = 0; i < 5; i++)
            {
                vouchersService.CreateForUser(user.Email);
            }

            var allVoucherId = dbContext.Vouchers.Where(x => x.ApplicationUserId == user.Id).ToList();
            allVoucherId[0].Status = VoucherStatus.Used;

            var expected = allVoucherId.Skip(1).Select(x => x.Id);
            var result = vouchersService.GetAllActiveForUser(user.Email).Select(x => x.Id);

            Assert.Equal(expected,result);
        }

        [Fact]
        public void GetAllShould_ReturAllVouchers()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_AllVouchers")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user1 = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var user2 = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test2@test.bg",
                FirstName = "Admin2",
                LastName = "LastAdmin2"
            };

            dbContext.Users.Add(user1);
            dbContext.Users.Add(user2);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(It.IsAny<string>())).
                                Returns((string res) => res);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            vouchersService.CreateForUser(user1.Email).GetAwaiter().GetResult();
            vouchersService.CreateForUser(user2.Email).GetAwaiter().GetResult();
            vouchersService.CreateForUser(user1.Email).GetAwaiter().GetResult();

            var expected = new List<string> { user1.Email, user2.Email, user1.Email };
            var result = dbContext.Vouchers.Select(x => x.ApplicationUserId);

            Assert.Equal(expected, result);
        }


        [Fact]
        public void GetDiscountForCodeShould_ReturnRightDiscountPercent()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DiscountValid")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(It.IsAny<string>())).
                Returns((string res) => res);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            var random = new Random();
            var discount = random.Next(1, 5);

            var voucher = new Voucher
            {
                ApplicationUserId = user.Id,
                Discount = discount,
                VoucherCode = Guid.NewGuid().ToString(),
                Status = VoucherStatus.Active
            };

            dbContext.Vouchers.Add(voucher);
            dbContext.SaveChanges();

            var expected = discount;
            var result = vouchersService.GetDiscountForCode(voucher.VoucherCode);

            Assert.Equal(expected, result);
        }


        [Fact]
        public void GetDiscountForCodeShould_0IfInvalidCode()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DiscountInvalid")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                                Returns(user.Id);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            var result = vouchersService.GetDiscountForCode(Guid.NewGuid().ToString());

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetDiscountForCodeShould_0IfEmptyString()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_DiscountInvalid_EmptyString")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                Returns(user.Id);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            var result = vouchersService.GetDiscountForCode(String.Empty);

            Assert.Equal(0, result);
        }

        [Fact]
        public void UserVoucherShould_MakeTheVoucherUsed()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_UseVoucher")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                Returns(user.Id);

            var random = new Random();
            var discount = random.Next(1, 5);

            var voucher = new Voucher
            {
                ApplicationUserId = user.Id,
                Discount = discount,
                VoucherCode = Guid.NewGuid().ToString(),
                Status = VoucherStatus.Active
            };

            dbContext.Vouchers.Add(voucher);
            dbContext.SaveChanges();

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            vouchersService.UseVoucher(voucher.VoucherCode);

            var expected = VoucherStatus.Used;
            var result = dbContext.Vouchers.FirstOrDefault(x => x.Id == voucher.Id).Status;

            Assert.Equal(expected, result);
        }


        [Fact]
        public void UserVoucherShould_ReturnFalseIfInvalidCode()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_UseInvalidVoucher")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(p => p.GetUserIdByEmail(user.Email)).
                Returns(user.Id);

            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            var result = vouchersService.UseVoucher(Guid.NewGuid().ToString()).GetAwaiter().GetResult();

            Assert.False(result);
        }

        [Fact]
        public void GetAllForUserShould_ReturnAllVouchersForUser()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_AllForUser")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);

            var usersServiceMock = new Mock<IUsersService>();
            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            var expectedVouchersCount = 5;
            for (int i = 0; i < expectedVouchersCount; i++)
            {
                vouchersService.CreateForUser(user.Email);
            }

            var expected = dbContext.Vouchers.
                Where(x => x.User.Email == user.Email).
                Count();

            var result = vouchersService.GetAllForUser(user.Email).Count;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAllVouchersShould_ReturnAllVouchers()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_GetAllVouchers")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var user2 = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test2@test.bg",
                FirstName = "Admin2",
                LastName = "LastAdmin2"
            };

            dbContext.Users.Add(user);
            dbContext.Users.Add(user2);

            var usersServiceMock = new Mock<IUsersService>();
            var vouchersService = new VouchersService(dbContext, usersServiceMock.Object, this.mapper);

            vouchersService.CreateForUser(user.Email);
            vouchersService.CreateForUser(user2.Email);

            var expected = dbContext.Vouchers.Count();
            var result = vouchersService.GetAllVouchers().Count;

            Assert.Equal(expected, result);
        }
    }
}
