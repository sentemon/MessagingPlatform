using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.UpdateUserPermissions;

public class UpdateUserPermissionsCommandHandler : ICommandHandler<UpdateUserPermissionsCommand, UserChat>
{
    public async Task<IResult<UserChat, Error>> Handle(UpdateUserPermissionsCommand command)
    {
        throw new NotImplementedException();
    }
}