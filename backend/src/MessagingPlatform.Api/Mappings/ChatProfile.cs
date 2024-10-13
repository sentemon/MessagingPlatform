using AutoMapper;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Api.Mappings;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<CreateChatDto, Chat>();

        CreateMap<Chat, GetChatDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title ?? "New Chat"))
            .ForMember(dest => dest.ChatType, opt => opt.MapFrom(src => (int)src.ChatType))
            .ForMember(dest => dest.UserChats, opt => opt.MapFrom(src => src.UserChats))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages));
    }
}