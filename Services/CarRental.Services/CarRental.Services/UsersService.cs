using CarRental.Models;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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

        public string GetUserIdByEmail(string email)
        {
            var user = this.userManager.FindByNameAsync(email).GetAwaiter().GetResult();
            return user.Id;
        }
    }
}
