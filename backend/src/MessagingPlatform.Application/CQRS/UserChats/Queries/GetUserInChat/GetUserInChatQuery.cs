using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUserInChat;

public record GetUserInChatQuery(Guid ChatId) : IQuery, ICommand;