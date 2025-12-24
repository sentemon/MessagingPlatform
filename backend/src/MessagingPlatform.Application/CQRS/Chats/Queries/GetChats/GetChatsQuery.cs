using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;

public record GetChatsQuery(Guid UserId) : IQuery;