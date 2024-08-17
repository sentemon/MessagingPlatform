namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class UpdateChatDto
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public required string Title { get; set; }
}