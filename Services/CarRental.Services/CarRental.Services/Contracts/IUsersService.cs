using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Services.Contracts
{
   public interface IUsersService
    {
        string GetUserIdByEmail(string email);
    
    }
}
