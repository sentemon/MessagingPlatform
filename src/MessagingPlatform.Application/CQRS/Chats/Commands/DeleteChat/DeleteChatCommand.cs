using MediatR;
using MessagingPlatform.Application.Common.Models.ChatDTOs;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;

public record DeleteChatCommand(DeleteChatDto DeleteChat) : IRequest<bool>;