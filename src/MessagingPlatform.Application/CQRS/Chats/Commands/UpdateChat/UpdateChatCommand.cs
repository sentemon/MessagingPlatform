using MediatR;
using MessagingPlatform.Application.Common.Models.ChatDTOs;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;

public record UpdateChatCommand(UpdateChatDto UpdateChat) : IRequest<string>;