using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Domain.Entities;

public class Group
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = "New Group";

    public List<User> Users { get; set; } = [];

    public List<Message>? Messages { get; set; } = [];

    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
}