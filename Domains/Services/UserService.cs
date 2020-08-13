using UsersList.Common.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using UsersList.Common.Models.Entities;
using Newtonsoft.Json;
using UsersList.Common.Services;

namespace UsersList.Services
{
    public class UserService : BaseHttpService, IUserService
    {

        public UserService() : base("user")
        {
        }

        public Task<int> Create(User newUser)
        {
            return PostAsync<int, User>("", newUser);
        }

        public Task<IEnumerable<User>> Get()
        {
            return GetAsync<IEnumerable<User>>();
        }

        public Task<User> Get(int id)
        {
            return GetAsync<User>($"/{id}");
        }

        public Task<int> Edit(User user)
        {
            return PutAsync<int, User>("", user);
        }

        public Task<int> Delete(int id)
        {
            return DeleteAsync<int>($"/{id}");
        }
    }
}
