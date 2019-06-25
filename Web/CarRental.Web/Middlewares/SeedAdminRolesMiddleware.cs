using CarRental.Common;
using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.Middlewares
{
    public class SeedAdminRolesMiddleware
    {
        private readonly RequestDelegate next;

        public SeedAdminRolesMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager,
                                     RoleManager<ApplicationRole> roleManager, CarRentalDbContext dbContext)
        {
            SeedRoles(roleManager).GetAwaiter().GetResult();

            SeedUserInRoles(userManager).GetAwaiter().GetResult();

            await next(context);
        }

        private static async Task SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(GlobalConstants.AdministratorRoleName))
            {
                await roleManager.CreateAsync(new ApplicationRole(GlobalConstants.AdministratorRoleName));
            }

            if (!await roleManager.RoleExistsAsync(GlobalConstants.UserRoleName))
            {
                await roleManager.CreateAsync(new ApplicationRole(GlobalConstants.UserRoleName));
            }
        }

        private static async Task SeedUserInRoles(UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Count() == 1)
            {
                var user = userManager.Users.FirstOrDefault();
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
