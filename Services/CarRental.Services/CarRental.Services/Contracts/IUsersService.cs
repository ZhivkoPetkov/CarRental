using CarRental.DTOs.Users;
using CarRental.Models;
using System.Collections.Generic;

namespace CarRental.Services.Contracts
{
   public interface IUsersService
    {
        string GetUserIdByEmail(string email);
        ApplicationUser GetUserByEmail(string email);
        ICollection<UserVouchersDto> GetAllUsers();
    }
}
