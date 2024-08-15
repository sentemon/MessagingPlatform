namespace MessagingPlatform.Application.Common.Models.MessageDTOs;

public class UpdateMessageDto
{
    public Guid SenderId { get; set; }
    public Guid MessageId { get; set; }
    public string Content { get; set; }
    public DateTime UpdatedAt { get; set; }
}