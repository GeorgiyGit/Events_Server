using Core.DTOs.UserDTOs;
using Core.Models;

namespace Core.Helpers
{
    internal class UserProfile:AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<UserCreateDTO, User>();

            CreateMap<UserEditDTO, User>();

            CreateMap<User, UserBase>();
        }
    }
}
