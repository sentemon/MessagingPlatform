using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.Services;

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
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            ChatType = ChatType.Private,
            Title = "Private Chat"
        };

        var fistUserChat = new UserChat
        {
            UserId = firstUserId,
            ChatId = chat.Id,
            Role = ChatRole.Owner,
            JoinedAt = DateTime.UtcNow,
            User = (await _userRepository.GetByIdAsync(firstUserId))!,
            Chat = chat

        };

        var secondUserChat = new UserChat
        {
            UserId = secondUserId,
            ChatId = chat.Id,
            Role = ChatRole.Owner,
            JoinedAt = DateTime.UtcNow,
            User = (await _userRepository.GetByIdAsync(secondUserId))!,
            Chat = chat
        };
        
        chat.UserChats?.Add(fistUserChat);
        chat.UserChats?.Add(secondUserChat);
        
        await _chatRepository.CreateAsync(chat);
        await _chatRepository.SaveChangesAsync();

        return chat;
    }
    
    public async Task<Chat> CreateGroupChatAsync(Guid creatorId, List<Guid> memberUserIds)
    {
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            ChatType = ChatType.Group,
            Title = "Group Chat"
        };
        
        var ownerUserChat = new UserChat
        {
            UserId = creatorId,
            ChatId = chat.Id,
            Role = ChatRole.Owner,
            JoinedAt = DateTime.UtcNow,
            User = await _userRepository.GetByIdAsync(creatorId)!,
            Chat = chat
        };
        
        chat.UserChats?.Add(ownerUserChat);

        foreach (var memberUserId in memberUserIds)
        {
            var memberUserChat = new UserChat
            {
                UserId = memberUserId,
                ChatId = chat.Id,
                Role = ChatRole.Member,
                JoinedAt = DateTime.UtcNow,
                User = await _userRepository.GetByIdAsync(memberUserId)!,
                Chat = chat
            };
            
            chat.UserChats?.Add(memberUserChat);
        }

        await _chatRepository.CreateAsync(chat);
        await _chatRepository.SaveChangesAsync();

        return chat;
    }

    public async Task<Chat> CreateChannelChatAsync(Guid creatorId, List<Guid> viewerUserIds)
    {
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            ChatType = ChatType.Channel,
            Title = "Channel Chat"
        };
        
        var ownerUserChat = new UserChat
        {
            UserId = creatorId,
            ChatId = chat.Id,
            Role = ChatRole.Owner,
            JoinedAt = DateTime.UtcNow,
            User = await _userRepository.GetByIdAsync(creatorId)!,
            Chat = chat
        };
        
        chat.UserChats?.Add(ownerUserChat);
        
        foreach (var viewerUserId in viewerUserIds)
        {
            var memberUserChat = new UserChat
            {
                UserId = viewerUserId,
                ChatId = chat.Id,
                Role = ChatRole.Viewer,
                JoinedAt = DateTime.UtcNow,
                User = await _userRepository.GetByIdAsync(viewerUserId)!,
                Chat = chat
            };
            
            chat.UserChats?.Add(memberUserChat);
        }

        await _chatRepository.CreateAsync(chat);
        await _chatRepository.SaveChangesAsync();

        return chat;
    }
}
