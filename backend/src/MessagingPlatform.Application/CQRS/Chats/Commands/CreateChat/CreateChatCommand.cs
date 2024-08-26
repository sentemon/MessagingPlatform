using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;

public record CreateChatCommand(Chat Chat, List<string> Usernames, Guid CreatorId) : IRequest<Chat>;