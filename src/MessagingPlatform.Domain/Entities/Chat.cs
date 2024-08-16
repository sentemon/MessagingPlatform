using System.ComponentModel.DataAnnotations;
using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; }

    [Required]
    public ChatType ChatType { get; set; }

    public List<User> Users { get; set; } = new();

    public List<Message>? Messages { get; set; } = new();

    public Guid CreatorId { get; set; }
    public User Creator { get; set; }

    [MaxLength(100)]
    public string? Title { get; set; }
}