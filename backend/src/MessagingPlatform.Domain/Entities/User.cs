using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required string Username { get; set; }
    
    public required string Email { get; set; }
    
    public required string PasswordHash { get; set; }
    
    public string? Bio { get; set; }
    
    public bool? IsOnline { get; set; }

    public required DateTime AccountCreatedAt { get; init; }
    
    public ICollection<UserChat>? UserChats { get; set; } = [];
    public ICollection<Message>? Messages { get; set; } = [];
}