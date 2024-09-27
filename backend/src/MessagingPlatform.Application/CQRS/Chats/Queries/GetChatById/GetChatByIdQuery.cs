using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById;

public record GetChatByIdQuery(Guid ChatId, Guid UserId) : IRequest<Chat?>;
