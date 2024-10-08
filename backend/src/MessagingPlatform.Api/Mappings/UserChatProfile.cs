using AutoMapper;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Application.Common.Models.UserChatDTOs;

namespace MessagingPlatform.Api.Mappings;


public class UserChatMappingProfile : Profile
{
    public UserChatMappingProfile()
    {
        CreateMap<UserChat, UserChatDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.ChatId))
            .ForMember(dest => dest.JoinedAt, opt => opt.MapFrom(src => src.JoinedAt));
    }
}
