using MediatR;
using MessagingPlatform.Application.Common.Models.UserChatDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.UpdateUserPermissions;

public record UpdateUserPermissionsCommand(Guid ChatId, Guid UserId, UpdateUserPermissionsDto Dto) : IRequest<UserChat>;