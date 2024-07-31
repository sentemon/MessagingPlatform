namespace MessagingPlatform.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; }
    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }
    public List<Message> Messages { get; set; } = new List<Message>();
}