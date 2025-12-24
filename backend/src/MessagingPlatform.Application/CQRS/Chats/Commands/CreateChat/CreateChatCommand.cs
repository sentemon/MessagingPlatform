using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;

public record CreateChatCommand(ChatType ChatType, List<string> Usernames, Guid CreatorId) : ICommand;