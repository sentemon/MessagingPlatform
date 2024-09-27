namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class UpdateChatDto
{
    // ToDo: add more fields
    public Guid ChatId { get; set; }
    public required string Title { get; set; }
}