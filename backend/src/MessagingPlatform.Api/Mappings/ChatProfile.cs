using AutoMapper;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Api.Mappings;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<CreateChatDto, Chat>();
    }
}