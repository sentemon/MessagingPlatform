using MediatR;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Messages.Queries.GetMessagesByUserIdAndChatId;

public record GetMessagesByUserIdAndChatIdQuery(Guid UserId, Guid ChatId) : IRequest<IQueryable<Message>>;