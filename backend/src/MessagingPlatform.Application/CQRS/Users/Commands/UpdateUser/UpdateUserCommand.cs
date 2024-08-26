using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<bool>
{
    public UpdateUserCommand(User updateUser)
    {
        UpdateUser = updateUser;
    }

    public User UpdateUser { get; set; }
}