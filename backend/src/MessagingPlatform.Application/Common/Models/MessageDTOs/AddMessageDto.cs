namespace MessagingPlatform.Application.Common.Models.MessageDTOs;

public class AddMessageDto
{
    public required Guid ChatId { get; set; }
    public required string Content { get; set; }
}