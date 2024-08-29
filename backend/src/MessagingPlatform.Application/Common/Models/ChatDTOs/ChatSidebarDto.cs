namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class ChatSidebarDto
{
    public Guid ChatId { get; set; }
    public string Title { get; set; } = "New Chat";
    public string? LastMessageFrom { get; set; }
    public string LastMessageContent { get; set; } = "Start a Conversation";
    public DateTime? LastMessageSentAt { get; set; }
    public int UnreadMessagesCount { get; set; }
}
