namespace MessagingPlatform.Application.Common.Models.MessageDTOs;

public class MessageDto
{
    public required string Sender { get; init; } // Sender Full Name
    public required string Content { get; set; }
    public required DateTime SentAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public bool? IsRead { get; set; } = false;
}