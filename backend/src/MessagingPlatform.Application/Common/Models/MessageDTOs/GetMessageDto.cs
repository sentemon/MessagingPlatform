namespace MessagingPlatform.Application.Common.Models.MessageDTOs;

public class GetMessageDto
{
    public required string SenderFullName { get; init; }
    public required string Content { get; set; }
    
    public required DateTime SentAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsRead { get; set; } = false;
}