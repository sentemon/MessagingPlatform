using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Domain.Interfaces;

public interface IChatRepository
{
    Task<Chat> CreateChatAsync(Guid creatorId, List<Guid> userIds, ChatType chatType, string? title = null);
    Task<Chat> GetChatByIdAsync(Guid chatId);
}