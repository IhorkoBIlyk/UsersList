using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Data.Repositories
{
    internal class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        public UserRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }

        public async Task<int> CreateUser(User newUser)
        {
            var result = await _applicationContext.Set<User>().AddAsync(newUser);
            return result.Entity.Id;
        }
    }
}

