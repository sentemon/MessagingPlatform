using AutoMapper;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Api.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, User>();
    }
}