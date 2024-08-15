namespace MessagingPlatform.Application.Common.Models.MessageDTOs;

public class DeleteMessageDto
{
    public required Guid SenderId { get; set; }
    public required Guid MessageId { get; set; }
}