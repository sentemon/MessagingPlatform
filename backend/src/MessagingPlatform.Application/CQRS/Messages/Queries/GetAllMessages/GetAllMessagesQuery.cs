using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Messages.Queries.GetAllMessages;

public class GetAllMessagesQuery : IRequest<IQueryable<Message>>;