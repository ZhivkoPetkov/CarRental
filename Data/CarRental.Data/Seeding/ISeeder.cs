namespace CarRental.Data.Seeding
{
    using CarRental.Models;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(CarRentalDbContext dbContext, IServiceProvider serviceProvider);
    }
}
