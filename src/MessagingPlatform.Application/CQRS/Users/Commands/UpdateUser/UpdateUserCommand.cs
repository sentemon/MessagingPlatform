using MediatR;
using MessagingPlatform.Application.Common.Models;

namespace MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public UpdateUserCommand(UpdateUserDto updateUser)
    {
        UpdateUser = updateUser;
    }

    public UpdateUserDto UpdateUser { get; set; }
}