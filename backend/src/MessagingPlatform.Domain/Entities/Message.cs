using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Domain.Entities;

public class Message
{
    public Guid Id { get; init; }

    [Required]
    public required Guid SenderId { get; init; }

    [Required]
    public required User Sender { get; init; }

    [Required]
    public required Guid ChatId { get; init; }

    [Required]
    public required Chat Chat { get; init; }

    [Required]
    public required string Content { get; set; }

    [Required]
    public required DateTime SentAt { get; init; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsRead { get; set; } = false;
}