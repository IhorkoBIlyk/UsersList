using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;
using Web.Data.Repositories;
using Web.DTO;
using Web.Models;
using Web.Service.Interfaces;

namespace Web.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<IUserRepository>();
        }

        public async Task<int> Create(UserDTO newUser)
        {
            var user = new User();
            user.Name = newUser.Name;
            var id = await _userRepository.CreateUser(user);
            await _unitOfWork.Save();

            return id;
        }

        public async Task DeleteRange(IEnumerable<UserDTO> users)
        {
            var usersToDelete = new List<User>();
            foreach (var user in users)
            {
                User userDTO = new User();
                userDTO.Id = user.Id;
                userDTO.Name = user.Name;
                usersToDelete.Add(userDTO);
            }

            _userRepository.DeleteRange(usersToDelete);

            await _unitOfWork.Save();
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _userRepository.GetById(id);
            if (entity == null)
            {
                return false;
            }
            _userRepository.Delete(entity);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Edit(int id, UserDTO newUser)
        {
            var entity = await _userRepository.GetById(id);
            if (entity != null)
            {
                entity.Name = newUser.Name;
                _userRepository.Update(entity);
                await _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public async Task<List<UserDTO>> Get()
        {
            //TODO ADD Automapper
            var list = await _userRepository.GetAll();

            List<UserDTO> result = new List<UserDTO>();

            foreach (var item in list)
            {
                var user = new UserDTO();
                user.Id = item.Id;
                user.Name = item.Name;

                result.Add(user);
            }

            return result;
        }

        public async Task<UserDTO> Get(int id)
        {
            //TODO ADD Automapper
            var user = await _userRepository.GetById(id);

            var result = new UserDTO();
            result.Id = user.Id;
            result.Name = user.Name;

            return result;
        }
    }
}
