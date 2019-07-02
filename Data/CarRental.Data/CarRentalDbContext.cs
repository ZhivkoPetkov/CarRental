namespace CarRental.Data
{
    using CarRental.Data.Common.Models;
    using CarRental.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class CarRentalDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(CarRentalDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        public DbSet<CarRentDays> CarRentDays { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
             => optionsBuilder
        .UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);


            builder.Entity<Car>().
                HasMany(x => x.RentDays).
                WithOne(x => x.Car).
                HasForeignKey(k => k.CarId);

            builder.Entity<ApplicationUser>().
               HasMany(x => x.Orders).
               WithOne(x => x.User).
               HasForeignKey(k => k.ApplicationUserId).
               OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Car>().
             HasMany(x => x.RentDays).
             WithOne(x => x.Car).
             HasForeignKey(k => k.CarId).
             OnDelete(DeleteBehavior.Restrict);



            builder.Entity<Location>().HasData(new Location { Id = 1, Name = "Sofia, Airport Terminal 1" });
            builder.Entity<Location>().HasData(new Location { Id = 2, Name = "Sofia, Airport Terminal 2" });
            builder.Entity<Location>().HasData(new Location { Id = 3, Name = "Plovdiv, Novotel" });
            builder.Entity<Location>().HasData(new Location { Id = 4, Name = "Stara Zagora, Stadium Beroe" });
            builder.Entity<Location>().HasData(new Location { Id = 5, Name = "Shumen, Post Office" });
            builder.Entity<Location>().HasData(new Location { Id = 6, Name = "Asenovgrad, Post Office" });
            builder.Entity<Location>().HasData(new Location { Id = 7, Name = "Varna, Bay" });
            builder.Entity<Location>().HasData(new Location { Id = 8, Name = "Pleven, Hotel Bulgaria" });
            builder.Entity<Location>().HasData(new Location { Id = 9, Name = "Sozopol, Old City Post Office" });
            builder.Entity<Location>().HasData(new Location { Id = 10, Name = "Lovech" });


            builder.Entity<Car>().HasData(new Car
            {
                Id = 1,
                Model = "Mazda 6",
                Description = "The 2019 Mazda 6 doesn’t make a bad step. This year, the mid-size sedan returns mostly unchanged from last year’s version, albeit with standard safety hardware that was optional last year.",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 1,
                PricePerDay = 65,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561808448/Mazda%206.jpg",
                Year = 2019
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 2,
                Model = "Mazda 3",
                Description = "The Mazda 3 is a family hatch, not an SUV or a crossover or pretending to be something it’s not. These days you don’t go to the expense of creating a whole new platform from the ground up without doing more than one thing with it, though, so expect more to come from the 3’s box of bits.",
                GearType = Models.Enums.GearType.Manual,
                LocationId = 1,
                PricePerDay = 39,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561808495/Mazda%203.jpg",
                Year = 2019
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 3,
                Model = "BMW X7",
                Description = "The X7 by contrast is about luxury. It takes themes from the facelifted 7 Series and the 8er, to make BMW’s three-flagship fleet. They want us to see this top-end trio as a separate high-end luxury series.",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 1,
                PricePerDay = 80,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561834504/BMW%20X7.webp",
                Year = 2019
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 4,
                Model = "Tesla Model X",
                Description = "The big Tesla. The one that seats seven and has every other firm on the planet that builds premium SUVs in a bit of a panic. Underneath, the architecture is similar to the Model S (massive battery pack, electric motors, aluminium chassis), but all Model X are four-wheel drive, so have twin electric motors, one driving each axle. ",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 1,
                PricePerDay = 80,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561928423/Tesla%20X.jpg",
                Year = 2019
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 5,
                Model = "Toyota Yaris iA",
                Description = "Developed by Mazda, launched by Scion, and now marketed as a Toyota, the Yaris iA proves that subcompact cars can delight. A different model from the Toyota Yaris hatchback, the frisky iA sedan stands out in a segment filled with insubstantial models. It feels refined for this entry-level class, with a smooth and willing four-cylinder engine, slick six-speed automatic transmission, and relatively compliant ride.",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 1,
                PricePerDay = 42,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561931317/Toyota-Yaris-02-17_zq9xxm.jpg",
                Year = 2017
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 6,
                Model = "Kia Optima",
                Description = "The Optima is a vehicle that delivers all of these virtues in a stylish, value-laden package that’s filled with features usually found on pricier cars. With outstanding reliability and extensive warranty coverage, savvy sedan shoppers should take this recently redesigned car for a test drive.",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 2,
                PricePerDay = 36,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561931416/Kia-02-17_c1aqgf.jpg",
                Year = 2017
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 7,
                Model = "Subaru Forester",
                Description = "The Subaru Forester sets the standard for small SUVs, combining relatively roomy packaging, fuel efficiency, solid reliability, and easy access. Large windows and a boxy shape maximize room for passengers and gear in sharp contrast to style trends exhibited by competitors that compromise practicality. ",
                GearType = Models.Enums.GearType.Manual,
                LocationId = 3,
                PricePerDay = 50,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561931481/Sub-Forrester-02-17_zkc3ya.jpg",
                Year = 2017
            });


            builder.Entity<Car>().HasData(new Car
            {
                Id = 8,
                Model = "Honda RidgeLine",
                Description = "Innovation abounds in this suburbia-targeted pickup, proving that trucks can be both refined and versatile. The Ridgeline glides along, more akin to a sedan than its roughneck rivals. It also handles far better than any compact or full-sized pickup, and it shames all nondiesel trucks for fuel economy. ",
                GearType = Models.Enums.GearType.Manual,
                LocationId = 4,
                PricePerDay = 20,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932112/Ridgeline-02-17_vfvtab.jpg",
                Year = 2017
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 9,
                Model = "Opel Insignia",
                Description = "The Insignia was the flagship of the Opel range and offered as a medium-large sedan and station wagon. Passenger space is good, with almost as much legroom, but slightly less width in the back seat than Commodore and Falcon.",
                GearType = Models.Enums.GearType.Manual,
                LocationId = 5,
                PricePerDay = 10,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932246/opel-insignia-11_s9rcid.png",
                Year = 2013
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 10,
                Model = "Chevrolet Impala",
                Description = "The Impala continues to reign as the leading large sedan. Slide behind the wheel and you can see why. Roomy, supportive seats put you in the perfect position to access the intuitive controls. Despite its prodigious size, the Impala’s handling is responsive and secure.",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 6,
                PricePerDay = 10,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932309/CR-Inline-top-picks-Chevy-Impala-02-17_cm6b9y.jpg",
                Year = 2017
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 11,
                Model = "Toyota Prius",
                Description = "The car that pioneered the hybrid movement and has defined fuel-efficiency for four model generations still stands tall as an innovative green machine. Its fuel economy in our tests was a staggering 52 mpg overall—the highest we’ve ever recorded in a car that doesn’t plug in.",
                GearType = Models.Enums.GearType.Automatic,
                LocationId = 7,
                PricePerDay = 39,
                Image = "https://res.cloudinary.com/dis59vn8s/image/upload/v1561932389/CR-Inline-top-picks-Toyota-Prius-02-17_cvg5ta.jpg",
                Year = 2017
            });

            ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void ConfigureUserIdentityRelations(ModelBuilder builder)
        {

        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
