using MediatR;
using MessagingPlatform.Application.Common.Models.ChatDTOs;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;

public record GetChatsQuery(Guid UserId) : IRequest<IEnumerable<ChatSidebarDto>>;