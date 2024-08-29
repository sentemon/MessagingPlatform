using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class DeleteChatDto
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; } // ToDo: delete
    
}