using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class CreateChatDto
{
    public string? Title { get; set; }
    public required ChatType ChatType { get; set; }
    public List<string> Users { get; set; } = []; // user usernames
}