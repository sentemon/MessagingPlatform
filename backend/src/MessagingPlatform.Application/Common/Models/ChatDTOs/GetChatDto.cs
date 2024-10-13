using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Application.Common.Models.UserChatDTOs;

namespace MessagingPlatform.Application.Common.Models.ChatDTOs;

public class GetChatDto
{
    public Guid Id { get; set; }
    public int ChatType { get; set; }
    public ICollection<GetUserChatDto>? UserChats { get; set; }
    public ICollection<GetMessageDto>? Messages { get; set; }
    public string Title { get; set; } = "New Chat";
}
