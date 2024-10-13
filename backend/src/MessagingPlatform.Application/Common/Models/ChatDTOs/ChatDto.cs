using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Application.Common.Models.UserChatDTOs;
using MessagingPlatform.Application.Common.Models.UserDTOs;

namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class ChatDto
{
    public Guid Id { get; set; }
    public int ChatType { get; set; }
    public ICollection<UserChatDto>? UserChats { get; set; }
    public ICollection<MessageDto>? Messages { get; set; }
    public string Title { get; set; } = "New Chat";
}
