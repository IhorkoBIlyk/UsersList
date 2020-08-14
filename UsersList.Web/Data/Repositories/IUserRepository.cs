using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        Task<int> CreateUser(User newUser);
    }
}
