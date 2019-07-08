namespace CarRental.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(CarRentalDbContext dbContext, IServiceProvider serviceProvider);
    }
}
