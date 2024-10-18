namespace MessagingPlatform.Application.Common.Models.MessageDTOs;

public class UpdateMessageDto
{
    public required Guid MessageId { get; set; }
    public required string Content { get; set; }
}