using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Extensions;

namespace MessagingPlatform.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; }
    
    public required ChatType ChatType { get; set; }

    public ICollection<UserChat>? UserChats { get; set; }

    public ICollection<Message>? Messages { get; set; }
    
    public string Title { get; set; } = "New Chat";
    
    private UserChat? GetUserChat(Guid userId)
    {
        return UserChats?.FirstOrDefault(uc => uc.UserId == userId);
    }
    
    private bool HasUserRights(Guid userId, ChatRights requiredRights)
    {
        var userChat = GetUserChat(userId);
        return userChat != null && userChat.Rights.HasRight(requiredRights);
    }

    public bool CanReadMessage(Guid userId) => HasUserRights(userId, ChatRights.Read);
    public bool CanSendMessage(Guid userId) => HasUserRights(userId, ChatRights.Read | ChatRights.Write);
    public bool CanUpdateMessage(Guid userId) => HasUserRights(userId, ChatRights.Read | ChatRights.Write | ChatRights.Update);
    public bool CanDeleteMessage(Guid userId) => HasUserRights(userId, ChatRights.Read | ChatRights.Write | ChatRights.Update | ChatRights.Delete);

    public bool CanAddUser()
    {
        return ChatType switch
        {
            ChatType.Private => (UserChats?.Count ?? 0) < 2,
            ChatType.Group => true,
            ChatType.Channel => false,
            _ => false
        };
    }

    public bool CanDeleteUser()
    {
        return ChatType switch
        {
            ChatType.Private => false,
            ChatType.Group => true,
            ChatType.Channel => true,
            _ => false
        };
    }
}