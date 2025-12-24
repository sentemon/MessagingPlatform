using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Application.Common.Models.UserChatDTOs;

public class UpdateUserPermissionsDto
{
	public ChatRights Rights { get; set; } = ChatRights.None;
	public ChatRole Role { get; set; } = ChatRole.Member;
}