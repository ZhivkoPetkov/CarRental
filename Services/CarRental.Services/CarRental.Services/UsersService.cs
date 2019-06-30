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

        string IUsersService.GetUserIdByEmail(string email)
        {
            return this.userManager.FindByEmailAsync(email).GetAwaiter().GetResult().Id;
        }
    }
}
