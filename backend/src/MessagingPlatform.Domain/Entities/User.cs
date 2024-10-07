using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }

    [Required]
    [MaxLength(30)]
    public required string Username { get; set; }

    [Required]
    [MaxLength(320)]
    public required string Email { get; set; }

    [Required]
    public required string PasswordHash { get; set; }

    public string? Bio { get; set; }
    public bool? IsOnline { get; set; }
    
    [Required]
    public required DateTime AccountCreatedAt { get; init; }

    public List<UserChat>? UserChats { get; set; } = [];
    public List<Message>? Messages { get; set; } = [];
}