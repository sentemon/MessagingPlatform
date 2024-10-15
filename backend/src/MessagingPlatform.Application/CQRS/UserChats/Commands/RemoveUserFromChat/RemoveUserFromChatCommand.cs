using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.RemoveUserFromChat;

public record RemoveUserFromChatCommand(Guid ChatId, Guid UserId) : IRequest<UserChat>;