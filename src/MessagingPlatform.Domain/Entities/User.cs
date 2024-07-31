namespace MessagingPlatform.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public bool IsOnline { get; set; }
    public List<Guid> FriendsId { get; set; } = new List<Guid>();
    public DateTime AccountCreatedAt { get; set; }
    
    public string ExternalAuthId { get; set; }
}