using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById;

public record GetChatByIdQuery(Guid ChatId, Guid UserId) : IQuery;
