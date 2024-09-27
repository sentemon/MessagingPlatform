using MediatR;
using MessagingPlatform.Application.Common.Models.ChatDTOs;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;

public record DeleteChatCommand(Guid ChatId, Guid UserId) : IRequest<bool>;