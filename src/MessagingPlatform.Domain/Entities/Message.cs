namespace MessagingPlatform.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public List<Guid> ReceiversId { get; set; } = new List<Guid>(); // receiver(s)
    public string Type { get; set; }
    public string Content { get; set; }
    public List<string> Details { get; set; } = new List<string>();
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}