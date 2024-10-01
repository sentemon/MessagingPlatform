using AutoMapper;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Api.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap()
            .ForMember(dest => dest.Username, opt => opt.Ignore())
            .ForMember(dest => dest.AccountCreatedAt, opt => opt.Ignore());

        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Username, opt => opt.Ignore());
    }
}