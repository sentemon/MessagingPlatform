using MessagingPlatform.Domain.Primitives;

namespace MessagingPlatform.Domain.Entities;

public sealed class Message : Entity
{
    private Message()
    {
    }

    private Message(User sender, Chat chat, string content, DateTime sentAtUtc, Guid? id = null)
    {
        Id = id == null || id == Guid.Empty ? Guid.NewGuid() : id.Value;
        SenderId = sender.Id;
        Sender = sender;
        ChatId = chat.Id;
        Chat = chat;
        Content = NormalizeContent(content);
        SentAt = sentAtUtc;
    }

    public Guid SenderId { get; private set; }

    public User Sender { get; private set; } = null!;

    public Guid ChatId { get; private set; }

    public Chat Chat { get; private set; } = null!;
    
    public string Content { get; private set; } = string.Empty;

    public DateTime SentAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }

    public bool IsRead { get; private set; }

    public static Message Create(User sender, Chat chat, string content, DateTime sentAtUtc, Guid? id = null)
    {
        ArgumentNullException.ThrowIfNull(sender);
        ArgumentNullException.ThrowIfNull(chat);

        return new Message(sender, chat, content, sentAtUtc, id);
    }

    public void UpdateContent(string content, DateTime updatedAtUtc)
    {
        Content = NormalizeContent(content);
        UpdatedAt = updatedAtUtc;
    }

    public void MarkRead()
    {
        IsRead = true;
    }

    public bool CanBeModifiedBy(Guid userId) => SenderId == userId;

    private static string NormalizeContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new DomainException("Message content cannot be empty.");
        }

        return content.Trim();
    }
}