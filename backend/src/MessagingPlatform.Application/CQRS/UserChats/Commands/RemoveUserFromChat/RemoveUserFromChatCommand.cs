using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.RemoveUserFromChat;

public record RemoveUserFromChatCommand(Guid ChatId, Guid UserId) : ICommand;