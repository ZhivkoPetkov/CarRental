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
                dbContext.Locations.Add(new Location { Name = "Stara Zagora, Stadium Beroe" });
                dbContext.Locations.Add(new Location { Name = "Shumen, Post Office" });
                dbContext.Locations.Add(new Location { Name = "Asenovgrad, Post Office" });
                dbContext.Locations.Add(new Location { Name = "Varna, Bay" });
                dbContext.Locations.Add(new Location { Name = "Pleven, Hotel Bulgaria" });
                dbContext.Locations.Add(new Location { Name = "Sozopol, Old City Post Office" });
                dbContext.SaveChanges();
            }

            if (!dbContext.Cars.Any())
            {
                dbContext.Cars.Add(new Car
                {
                    Model = "Mazda 6",
                    Description = "The 2019 Mazda 6 doesn’t make a bad step. This year, the mid-size sedan returns mostly unchanged from last year’s version, albeit with standard safety hardware that was optional last year.",
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = 1,
                    PricePerDay = 65,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561808448/Mazda%206.jpg",
                    Year = 2019
                });

                dbContext.Cars.Add(new Car
                {
                    Model = "Mazda 3",
                    Description = "The Mazda 3 is a family hatch, not an SUV or a crossover or pretending to be something it’s not. These days you don’t go to the expense of creating a whole new platform from the ground up without doing more than one thing with it, though, so expect more to come from the 3’s box of bits.",
                    GearType = Models.Enums.GearType.Manual,
                    LocationId = 1,
                    PricePerDay = 39,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561808495/Mazda%203.jpg",
                    Year = 2019
                });

                dbContext.Cars.Add(new Car
                {
                    Model = "BMW X7",
                    Description = "The X7 by contrast is about luxury. It takes themes from the facelifted 7 Series and the 8er, to make BMW’s three-flagship fleet. They want us to see this top-end trio as a separate high-end luxury series.",
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = 1,
                    PricePerDay = 80,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561834504/BMW%20X7.webp",
                    Year = 2019
                });

                dbContext.Cars.Add(new Car
                {
                    Model = "Tesla Model X",
                    Description = "The big Tesla. The one that seats seven and has every other firm on the planet that builds premium SUVs in a bit of a panic. Underneath, the architecture is similar to the Model S (massive battery pack, electric motors, aluminium chassis), but all Model X are four-wheel drive, so have twin electric motors, one driving each axle. ",
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = 1,
                    PricePerDay = 80,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561928423/Tesla%20X.jpg",
                    Year = 2019
                });

                dbContext.Cars.Add(new Car
                {
                    Model = "Toyota Yaris iA",
                    Description = "Developed by Mazda, launched by Scion, and now marketed as a Toyota, the Yaris iA proves that subcompact cars can delight. A different model from the Toyota Yaris hatchback, the frisky iA sedan stands out in a segment filled with insubstantial models. It feels refined for this entry-level class, with a smooth and willing four-cylinder engine, slick six-speed automatic transmission, and relatively compliant ride.",
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = 1,
                    PricePerDay = 42,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561928423/Tesla%20X.jpg",
                    Year = 2017
                });

                dbContext.Cars.Add(new Car
                {
                    Model = "Kia Optima",
                    Description = "The Optima is a vehicle that delivers all of these virtues in a stylish, value-laden package that’s filled with features usually found on pricier cars. With outstanding reliability and extensive warranty coverage, savvy sedan shoppers should take this recently redesigned car for a test drive.",
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = 2,
                    PricePerDay = 36,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561931416/Kia-02-17_c1aqgf.jpg",
                    Year = 2017
                });

                dbContext.Cars.Add(new Car
                {
                    Model = "Subaru Forester",
                    Description = "The Subaru Forester sets the standard for small SUVs, combining relatively roomy packaging, fuel efficiency, solid reliability, and easy access. Large windows and a boxy shape maximize room for passengers and gear in sharp contrast to style trends exhibited by competitors that compromise practicality. ",
                    GearType = Models.Enums.GearType.Manual,
                    LocationId = 3,
                    PricePerDay = 50,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561931481/Sub-Forrester-02-17_zkc3ya.jpg",
                    Year = 2017
                });


                dbContext.Cars.Add(new Car
                {
                    Model = "Honda RidgeLine",
                    Description = "Innovation abounds in this suburbia-targeted pickup, proving that trucks can be both refined and versatile. The Ridgeline glides along, more akin to a sedan than its roughneck rivals. It also handles far better than any compact or full-sized pickup, and it shames all nondiesel trucks for fuel economy. ",
                    GearType = Models.Enums.GearType.Manual,
                    LocationId = 4,
                    PricePerDay = 20,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932112/Ridgeline-02-17_vfvtab.jpg",
                    Year = 2017
                });

                dbContext.Cars.Add(new Car
                {
                    Model = "Opel Insignia",
                    Description = "The Insignia was the flagship of the Opel range and offered as a medium-large sedan and station wagon. Passenger space is good, with almost as much legroom, but slightly less width in the back seat than Commodore and Falcon.",
                    GearType = Models.Enums.GearType.Manual,
                    LocationId = 5,
                    PricePerDay = 10,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932246/opel-insignia-11_s9rcid.png",
                    Year = 2013
                });

                dbContext.Cars.Add(new Car
                {
                    Model = "Chevrolet Impala",
                    Description = "The Impala continues to reign as the leading large sedan. Slide behind the wheel and you can see why. Roomy, supportive seats put you in the perfect position to access the intuitive controls. Despite its prodigious size, the Impala’s handling is responsive and secure.",
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = 6,
                    PricePerDay = 10,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932309/CR-Inline-top-picks-Chevy-Impala-02-17_cm6b9y.jpg",
                    Year = 2017
                });

                dbContext.Cars.Add(new Car
                {
                    Model = "Toyota Prius",
                    Description = "The car that pioneered the hybrid movement and has defined fuel-efficiency for four model generations still stands tall as an innovative green machine. Its fuel economy in our tests was a staggering 52 mpg overall—the highest we’ve ever recorded in a car that doesn’t plug in.",
                    GearType = Models.Enums.GearType.Automatic,
                    LocationId = 7,
                    PricePerDay = 39,
                    Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932389/CR-Inline-top-picks-Toyota-Prius-02-17_cvg5ta.jpg",
                    Year = 2017
                });

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
