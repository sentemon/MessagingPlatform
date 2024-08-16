using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;

public record GetChatsQuery : IRequest<IEnumerable<Chat>>;