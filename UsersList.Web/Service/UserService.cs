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
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<IUserRepository>();
            _mapper = mapper;
        }

        public async Task<int> Create(UserDTO newUser)
        {
            var user = _mapper.Map<User>(newUser);
            var id = await _userRepository.CreateUser(user);
            await _unitOfWork.Save();

            return id;
        }

        public async Task DeleteRange(IEnumerable<UserDTO> users)
        {
            var usersToDelete = _mapper.Map<IEnumerable<User>>(users);

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

        public async Task<IEnumerable<UserDTO>> Get()
        {
            var list = await _userRepository.GetAll();
            var result = _mapper.Map<IEnumerable<UserDTO>>(list);
            return result;
        }

        public async Task<UserDTO> Get(int id)
        {
            var user = await _userRepository.GetById(id);
            var result = _mapper.Map<UserDTO>(user);

            return result;
        }
    }
}
