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
            SeedRolesAndLocations(roleManager, dbContext).GetAwaiter().GetResult();

            SeedUserInRoles(userManager, dbContext).GetAwaiter().GetResult();

            await next(context);
        }

        private static async Task SeedRolesAndLocations(RoleManager<ApplicationRole> roleManager,CarRentalDbContext dbContext)
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

        private static async Task SeedUserInRoles(UserManager<ApplicationUser> userManager, CarRentalDbContext dbContext)
        {

            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.bg",
                    Email = "admin@gmail.com",
                    FirstName = "Zhivko",
                    LastName = "Petkov",
                };
                var password = "123123";

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
            }
        }
    }
}
