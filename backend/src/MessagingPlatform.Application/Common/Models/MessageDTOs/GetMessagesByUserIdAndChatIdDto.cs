namespace MessagingPlatform.Application.Common.Models.MessageDTOs;

public class GetMessagesByUserIdAndChatIdDto
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
}