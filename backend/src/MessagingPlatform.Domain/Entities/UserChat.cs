using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Domain.Entities;

public sealed class UserChat
{
    private UserChat()
    {
    }

    private UserChat(User user, Chat chat, ChatRole role, ChatRights rights, DateTime joinedAtUtc)
    {
        User = user;
        Chat = chat;
        UserId = user.Id;
        ChatId = chat.Id;
        Role = role;
        Rights = rights;
        JoinedAt = joinedAtUtc;
    }

    public Guid UserId { get; private set; }
    
    public User User { get; private set; } = null!;
    
    public Guid ChatId { get; private set; }
    
    public Chat Chat { get; private set; } = null!;

    public DateTime JoinedAt { get; private set; }

    public ChatRights Rights { get; private set; } = ChatRights.None;

    public ChatRole Role { get; private set; }

    public static UserChat Create(User user, Chat chat, ChatRole role, ChatRights rights, DateTime joinedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(chat);

        return new UserChat(user, chat, role, rights, joinedAtUtc);
    }

    public void UpdatePermissions(ChatRights rights, ChatRole role)
    {
        Rights = rights;
        Role = role;
    }

    internal void Detach()
    {
        User = null!;
        Chat = null!;
    }
}