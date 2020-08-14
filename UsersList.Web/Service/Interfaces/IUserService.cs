using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DTO;
using Web.Models;

namespace Web.Service.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> Get();
        Task<UserDTO> Get(int id);
        Task<int> Create(UserDTO item);
        Task<bool> Edit(int id, UserDTO item);
        Task<bool> Delete(int id);
        Task DeleteRange(IEnumerable<UserDTO> users);
    }
}
