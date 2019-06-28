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

            if (!dbContext.Locations.Any())
            {
                dbContext.Locations.Add(new Location { Name = "Sofia, Airport Terminal 1" });
                dbContext.Locations.Add(new Location { Name = "Sofia, Airport Terminal 2" });
                dbContext.Locations.Add(new Location { Name = "Plovdiv, Novotel" });
                dbContext.Locations.Add(new Location { Name = "Plovdiv, Airport" });
                dbContext.Locations.Add(new Location { Name = "Stara Zagora" });
                dbContext.Locations.Add(new Location { Name = "Shumen" });
                dbContext.Locations.Add(new Location { Name = "Asenovgrad" });
                dbContext.Locations.Add(new Location { Name = "Varna, Bay" });
                dbContext.Locations.Add(new Location { Name = "Pleven" });
                dbContext.Locations.Add(new Location { Name = "Sozopol" });
                dbContext.SaveChanges();
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
