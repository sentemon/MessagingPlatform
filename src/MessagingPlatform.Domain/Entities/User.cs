using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Domain.Entities;

public class User
{
    public Guid Id { get; init; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(30)]
    public string Username { get; set; }

    [Required]
    [MaxLength(320)]
    public string Email { get; set; }
    
    [Required]
    
    public string PasswordHash { get; set; }
    
    public string? Bio { get; set; }
    public bool? IsOnline { get; set; }
    public required DateTime AccountCreatedAt { get; set; }

    public List<Chat>? Chats { get; set; }
    public List<Group>? Groups { get; set; }
    public List<User>? Friends { get; set; }
    public List<Message>? Messages { get; set; }
}