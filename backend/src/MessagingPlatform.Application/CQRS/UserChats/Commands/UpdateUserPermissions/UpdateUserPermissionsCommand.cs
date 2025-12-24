using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common.Models.UserChatDTOs;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.UpdateUserPermissions;

public record UpdateUserPermissionsCommand(Guid ChatId, Guid UserId, UpdateUserPermissionsDto Dto) : ICommand;