namespace MessagingPlatform.Domain.Entities;

public class Group
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "New Group";
    public List<Guid> UsersId { get; set; } = new List<Guid>();
    public List<Message>? Messages { get; set; } = new List<Message>();
    public Guid CreatorId { get; set; }
}