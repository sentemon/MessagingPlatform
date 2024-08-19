namespace MessagingPlatform.Application.Common.Models.MessageDTOs;

public class UpdateMessageDto
{
    public required Guid SenderId { get; set; }
    public required Guid MessageId { get; set; }
    public required string Content { get; set; }
    public required DateTime UpdatedAt { get; set; }
}