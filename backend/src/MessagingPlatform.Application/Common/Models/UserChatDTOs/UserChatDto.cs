using MessagingPlatform.Application.Common.Models.UserDTOs;

namespace MessagingPlatform.Application.Common.Models.UserChatDTOs;

public class UserChatDto
{
    public Guid UserId { get; set; }
    public UserDto User { get; set; }
    public Guid ChatId { get; set; }
    public DateTime JoinedAt { get; set; }
}
