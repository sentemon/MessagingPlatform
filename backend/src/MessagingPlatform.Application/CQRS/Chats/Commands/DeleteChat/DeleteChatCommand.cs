using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;

public record DeleteChatCommand(Guid ChatId, Guid UserId) : ICommand;