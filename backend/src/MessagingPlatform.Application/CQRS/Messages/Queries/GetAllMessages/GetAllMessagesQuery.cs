using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Messages.Queries.GetAllMessages;

public record GetAllMessagesQuery(Guid ChatId) : IRequest<IQueryable<Message>>;