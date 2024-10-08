using System.ComponentModel.DataAnnotations;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Extensions;

namespace MessagingPlatform.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; }

    [Required]
    public required ChatType ChatType { get; set; }

    public List<UserChat>? UserChats { get; set; } = [];

    public List<Message>? Messages { get; set; } = [];
    
    [Required]
    public required Guid CreatorId { get; set; }
    
    [Required]
    public required User Creator { get; set; }

    [MaxLength(50)]
    public string Title { get; set; } = "New Chat";
    
    private UserChat? GetUserChat(Guid userId)
    {
        return UserChats?.FirstOrDefault(uc => uc.UserId == userId);
    }

    public bool CanReadMessage(Guid userId)
    {
        var userChats = GetUserChat(userId);

        return userChats != null && userChats.Rights.HasRight(ChatRights.Read);
    }

    public bool CanSendMessage(Guid userId)
    {
        var userChats = GetUserChat(userId);

        return userChats != null && userChats.Rights.HasRight(ChatRights.Read | ChatRights.Write);
    }
    
    public bool CanUpdateMessage(Guid userId)
    {
        var userChats = GetUserChat(userId);

        return userChats != null && userChats.Rights.HasRight(ChatRights.Read | ChatRights.Write | ChatRights.Update);
    }
    
    public bool CanDeleteMessage(Guid userId)
    {
        var userChats = GetUserChat(userId);

        return userChats != null && userChats.Rights.HasRight(ChatRights.Read | ChatRights.Write | ChatRights.Update | ChatRights.Delete);
    }
    
    public bool CanAddUser()
    {
        return ChatType != ChatType.Private || (UserChats?.Count ?? 0) < 2;
    }
}