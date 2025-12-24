using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUsersInChat;

public record GetUsersInChatQuery(Guid ChatId) : IQuery;