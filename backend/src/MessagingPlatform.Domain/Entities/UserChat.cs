using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Domain.Entities;

public class UserChat
{
    public required Guid UserId { get; set; }
    
    public required User User { get; set; }
    
    public required Guid ChatId { get; set; }
    
    public required Chat Chat { get; set; }

    public DateTime JoinedAt { get; init; }

    public ChatRights Rights { get; set; } = ChatRights.None;

    public ChatRole Role { get; set; }
}