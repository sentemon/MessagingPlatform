using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUserInChat;

public record GetUserInChatQuery(Guid ChatId, string Username) : IQuery, ICommand;