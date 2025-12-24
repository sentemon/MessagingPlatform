using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common.Models.ChatDTOs;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;

public record UpdateChatCommand(Guid UserId, Guid ChatId, string Title) : ICommand;