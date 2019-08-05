using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CarRental.Services.Tests
{
    public class UsersServiceTests : BaseServiceTests
    {
        [Fact]
        public void GetUserByEmailShould_ReturnCorrectUser()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_GetUserByEmail")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userManager = MockUserManager<ApplicationUser>();
            userManager.Setup(x => x.FindByNameAsync(user.Email)).ReturnsAsync(user);

            var usersService = new UsersService(userManager.Object, dbContext);
            var foundUser = usersService.GetUserByEmail(user.Email);

            Assert.Equal(user.Id, foundUser.Id);
            Assert.Equal(user.Email,foundUser.Email);
        }

        [Fact]
        public void GetUserByEmailShould_ReturnNullIfInvalidEmail()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_GetUserByEmailInvalid")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userManager = MockUserManager<ApplicationUser>();
            userManager.Setup(x => x.FindByNameAsync(user.Email)).ReturnsAsync(user);

            var usersService = new UsersService(userManager.Object, dbContext);
            var foundUser = usersService.GetUserByEmail(Guid.NewGuid().ToString());

            Assert.Null(foundUser);
        }

        [Fact]
        public void GetUserIdByEmailShould_ReturnCorrectId()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_GetUserIdByEmail")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userManager = MockUserManager<ApplicationUser>();
            userManager.Setup(x => x.FindByEmailAsync(user.Email)).
                            ReturnsAsync(dbContext.Users.FirstOrDefault(x => x.Email == user.Email));

            var usersService = new UsersService(userManager.Object, dbContext);
            var foundUserId = usersService.GetUserIdByEmail(user.Email);

            Assert.Equal(user.Id, foundUserId);
        }

        [Fact]
        public void GetUserIdByEmailShould_ReturnNullIfInvalidEmail()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_GetUserIdByInvalidEmail")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userManager = MockUserManager<ApplicationUser>();
            userManager.Setup(x => x.FindByEmailAsync(user.Email)).
                ReturnsAsync(dbContext.Users.FirstOrDefault(x => x.Email == user.Email));

            var usersService = new UsersService(userManager.Object, dbContext);
            Assert.Throws<NullReferenceException>(() => usersService.GetUserIdByEmail(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void GetAllUsersShould_ReturnUsersInDatabase()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_GetAllUsers")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var user1 = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.bg",
                FirstName = "Admin",
                LastName = "LastAdmin"
            };

            var user2 = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test2@test.bg",
                FirstName = "Admin2",
                LastName = "LastAdmin2"
            };

            dbContext.Users.Add(user1);
            dbContext.Users.Add(user2);
            dbContext.SaveChanges();

            var userManager = MockUserManager<ApplicationUser>();
            var usersService = new UsersService(userManager.Object, dbContext);

            var exptected = new List<string> {user1.Email, user2.Email};
            var result = usersService.GetAllUsers().Select(x => x.Email).ToList();

            Assert.Equal(exptected,result);
        }

        [Fact]
        public void GetAllUsersShould_ReturnEmptyCollectionIfNoUsers()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "CarRental_Database_GetAllUsers")
                .Options;
            var dbContext = new CarRentalDbContext(options);

            var userManager = MockUserManager<ApplicationUser>();
            var usersService = new UsersService(userManager.Object, dbContext);

            var result = usersService.GetAllUsers().Select(x => x.Email).ToList();

            Assert.Empty(result);
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }
    }
}
