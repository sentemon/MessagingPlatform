using System.ComponentModel.DataAnnotations;
using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class CreateChatDto
{
    [Required]
    public required ChatType ChatType { get; set; }
    [Required]
    public required Guid CreatorId { get; set; }
    public string? Title { get; set; }
    [Required]
    public List<Guid> UserIds { get; set; } = new();
}