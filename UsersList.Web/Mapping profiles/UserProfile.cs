using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DTO;
using Web.Models;

namespace Web.Mapping_profiles
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>()
                .ForMember(
                    dest => dest.Id,
                    opts => opts.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opts => opts.MapFrom(src => src.Name));

            CreateMap<User, UserDTO>()
                .ForMember(
                    dest => dest.Id,
                    opts => opts.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opts => opts.MapFrom(src => src.Name));
        }
    }
}
