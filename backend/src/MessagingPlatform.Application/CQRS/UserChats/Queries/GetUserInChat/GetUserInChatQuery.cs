using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUserInChat;

public record GetUserInChatQuery(Guid ChatId) : IRequest<UserChat>;