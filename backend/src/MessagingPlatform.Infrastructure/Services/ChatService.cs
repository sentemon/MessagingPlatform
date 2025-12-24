using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Extensions;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;

    public ChatService(IChatRepository chatRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public async Task<Chat> CreatePrivateChatAsync(Guid firstUserId, Guid secondUserId)
    {
        var chat = Chat.Create(ChatType.Private, "Private Chat");

        var firstUser = await _userRepository.GetByIdAsync(firstUserId) ?? throw new ArgumentException("Invalid creator id.");
        var secondUser = await _userRepository.GetByIdAsync(secondUserId) ?? throw new ArgumentException("Invalid interlocutor id.");

        chat.AddParticipant(firstUser, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);
        chat.AddParticipant(secondUser, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);

        await _chatRepository.CreateAsync(chat);

        return chat;
    }
    
    public async Task<Chat> CreateGroupChatAsync(Guid creatorId, List<Guid> memberUserIds)
    {
        var chat = Chat.Create(ChatType.Group, "Group Chat");

        var owner = await _userRepository.GetByIdAsync(creatorId) ?? throw new ArgumentException("Invalid creator id.");
        chat.AddParticipant(owner, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);

        foreach (var memberUserId in memberUserIds.Distinct())
        {
            var member = await _userRepository.GetByIdAsync(memberUserId);
            chat.AddParticipant(member, ChatRole.Member, ChatRights.Read | ChatRights.Write, DateTime.UtcNow);
        }

        await _chatRepository.CreateAsync(chat);

        return chat;
    }

    public async Task<Chat> CreateChannelChatAsync(Guid creatorId, List<Guid> viewerUserIds)
    {
        var chat = Chat.Create(ChatType.Channel, "Channel Chat");

        var owner = await _userRepository.GetByIdAsync(creatorId) ?? throw new ArgumentException("Invalid creator id.");
        chat.AddParticipant(owner, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);
        
        foreach (var viewerUserId in viewerUserIds.Distinct())
        {
            var viewer = await _userRepository.GetByIdAsync(viewerUserId);
            chat.AddParticipant(viewer, ChatRole.Viewer, ChatRights.Read, DateTime.UtcNow);
        }

        await _chatRepository.CreateAsync(chat);

        return chat;
    }
}
