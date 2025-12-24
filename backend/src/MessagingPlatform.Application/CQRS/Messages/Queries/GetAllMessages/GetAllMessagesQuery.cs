using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Messages.Queries.GetAllMessages;

public record GetAllMessagesQuery(Guid ChatId) : IQuery;