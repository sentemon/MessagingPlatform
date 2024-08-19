using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Application.Common.Models.ChatDTOs;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;

public record CreateChatCommand(CreateChatDto CreateChat) : IRequest<Chat>;