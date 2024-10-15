using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.AddUserToChat;

public record AddUserToChatCommand : IRequest<UserChat>;