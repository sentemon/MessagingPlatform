using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.AddUserToChat;

public record AddUserToChatCommand(Guid ChatId, Guid UserId) : ICommand;