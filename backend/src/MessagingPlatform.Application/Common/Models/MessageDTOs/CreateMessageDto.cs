namespace MessagingPlatform.Application.Common.Models.MessageDTOs;

public class CreateMessageDto
{
    public required Guid ChatId { get; set; }
    public required string Content { get; set; }
}