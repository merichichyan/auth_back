using AutoMapper;
using auth_back.Models;
using auth_back.DTOs;

namespace auth_back.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        CreateMap<RegisterDto, User>();
    }
}
