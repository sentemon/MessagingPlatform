using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUsersInChat;

public record GetUsersInChatQuery(Guid ChatId) : IRequest<ICollection<UserChat>>;