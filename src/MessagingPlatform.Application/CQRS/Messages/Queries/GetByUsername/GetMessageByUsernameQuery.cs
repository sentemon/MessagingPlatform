using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Messages.Queries.GetByUsername;

public record GetMessageByUsernameQuery(string SenderUsername, string ReceiverUsername) : IRequest<IQueryable<Message>>;