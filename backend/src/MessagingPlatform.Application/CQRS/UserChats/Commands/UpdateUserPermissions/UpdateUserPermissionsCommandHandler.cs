using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.UpdateUserPermissions;

public class UpdateUserPermissionsCommandHandler : IRequestHandler<UpdateUserPermissionsCommand, UserChat>
{
    public async Task<UserChat> Handle(UpdateUserPermissionsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}