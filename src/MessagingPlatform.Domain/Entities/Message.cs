using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }

    public Guid SenderId { get; set; }
    public User Sender { get; set; }

    public List<User> Receivers { get; set; } = [];

    [Required]
    public string Type { get; set; }

    [Required]
    public string Content { get; set; }

    public List<string> Details { get; set; } = [];

    [Required]
    public DateTime SentAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsRead { get; set; }
}
