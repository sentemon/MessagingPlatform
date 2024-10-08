using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class CreateChatDto
{
    public string? Title { get; set; } = "New Chat";
    public required ChatType ChatType { get; set; }
    public List<string> UserUsernames { get; set; } = [];
}