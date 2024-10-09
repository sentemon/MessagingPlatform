namespace MessagingPlatform.Domain.Entities;

public class Message
{
    public Guid Id { get; init; }
    
    public required Guid SenderId { get; init; }
    
    public required User Sender { get; init; }

    public required Guid ChatId { get; init; }

    public required Chat Chat { get; init; }
    
    public required string Content { get; set; }

    public required DateTime SentAt { get; init; }
    
    public DateTime? UpdatedAt { get; set; }

    public bool IsRead { get; set; } = false;
}