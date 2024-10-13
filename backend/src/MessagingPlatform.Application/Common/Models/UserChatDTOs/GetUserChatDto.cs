using MessagingPlatform.Application.Common.Models.UserDTOs;

namespace MessagingPlatform.Application.Common.Models.UserChatDTOs;

public class GetUserChatDto
{
    public Guid UserId { get; set; }
    public GetUserDto GetUser { get; set; }
    public Guid ChatId { get; set; }
    public DateTime JoinedAt { get; set; }
}
