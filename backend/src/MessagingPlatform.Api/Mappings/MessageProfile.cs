using AutoMapper;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Api.Mappings;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Message, GetMessageDto>()
            .ForMember(dest => dest.SenderFullName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"));
    }
}