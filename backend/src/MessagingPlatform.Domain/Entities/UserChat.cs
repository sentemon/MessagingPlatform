using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Domain.Entities;

public class UserChat
{
    [Required]
    public required Guid UserId { get; set; }
    
    [Required]
    public required User User { get; set; }

    [Required]
    public required Guid ChatId { get; set; }
    
    [Required]
    public required Chat Chat { get; set; }

    public DateTime? JoinedAt { get; set; } = DateTime.UtcNow;
}