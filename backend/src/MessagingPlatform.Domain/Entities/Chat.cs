using System.ComponentModel.DataAnnotations;
using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Domain.Entities;

public class Chat
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required ChatType ChatType { get; set; }

    public List<UserChat>? UserChats { get; set; } = [];

    public List<Message>? Messages { get; set; } = [];
    
    [Required]
    public required Guid CreatorId { get; set; }
    
    [Required]
    public required User? Creator { get; set; }

    [MaxLength(50)]
    public string? Title { get; set; }
    
    public bool CanAddUser()
    {
        return ChatType != ChatType.Private || (UserChats?.Count ?? 0) < 2;
    }
}