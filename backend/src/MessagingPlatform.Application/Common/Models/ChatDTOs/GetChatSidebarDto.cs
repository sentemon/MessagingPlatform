namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class GetChatSidebarDto
{
    public Guid ChatId { get; set; }
    public string? Title { get; set; }
    public string? LastMessageFrom { get; set; }
    public string? LastMessageContent { get; set; }
    public DateTime? LastMessageSentAt { get; set; }
    public int UnreadMessagesCount { get; set; }
}
