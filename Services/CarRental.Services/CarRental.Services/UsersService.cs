using CarRental.DTOs.Users;
using CarRental.Models;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using CarRental.Data;

namespace CarRental.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CarRentalDbContext dbContext;

        public UsersService(UserManager<ApplicationUser> userManager, CarRentalDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return this.userManager.FindByNameAsync(email).GetAwaiter().GetResult();
        }

        public string GetUserIdByEmail(string email)
        {
            var user = this.userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            return user.Id;
        }

        public ICollection<UserVouchersDto> GetAllUsers()
        {
            var users = this.dbContext.Users.Select(x => new UserVouchersDto
            {
                Email = x.Email,
                MoneySpent = x.Orders.Sum(m => m.Price),
                Rents = x.Orders.Count()
            }).ToList();

            return users;
        }
    }
}
