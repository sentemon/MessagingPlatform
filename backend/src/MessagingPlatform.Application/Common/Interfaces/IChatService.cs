using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Application.Common.Interfaces;

public interface IChatService
{
    Task<Chat> CreatePrivateChatAsync(Guid firstUserId, Guid secondUserId);
    Task<Chat> CreateGroupChatAsync(Guid creatorUserId, List<Guid> memberUserIds);
    Task<Chat> CreateChannelChatAsync(Guid creatorUserId, List<Guid> viewerUserIds);
}