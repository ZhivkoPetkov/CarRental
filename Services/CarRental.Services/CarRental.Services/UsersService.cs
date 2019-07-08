using CarRental.Data;
using CarRental.DTOs.Users;
using CarRental.Models;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRental.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return this.userManager.FindByNameAsync(email).GetAwaiter().GetResult();
        }

        public string GetUserIdByName(string email)
        {
            return this.userManager.FindByNameAsync(email).GetAwaiter().GetResult().Id;
        }

        public string GetUserIdByEmail(string email)
        {
            var user = this.userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            return user.Id;
        }

        public ICollection<UserVouchersDto> GetAllUsers()
        {
            var users = this.userManager.Users.Select(x => new UserVouchersDto
            {
                Email = x.Email,
                MoneySpent = x.Orders.Sum(m => m.Price),
                Rents = x.Orders.Count()
            }).ToList();

            return users;
        }
    }
}
