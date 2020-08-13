using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsersList.Common.Models.Entities;

namespace UsersList.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> Get();
        Task<User> Get(int id);
        Task<int> Create(User item);
        Task<int> Edit(User item);
        Task<int> Delete(int id);
    }
}
