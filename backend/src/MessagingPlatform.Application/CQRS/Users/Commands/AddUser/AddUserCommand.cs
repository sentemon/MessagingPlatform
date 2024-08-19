using MediatR;
using MessagingPlatform.Application.Common.Models.UserDTOs;

namespace MessagingPlatform.Application.CQRS.Users.Commands.AddUser;

public class AddUserCommand : IRequest<bool>
{
    public AddUserCommand(AddUserDto addUser)
    {
        AddUser = addUser;
    }

    public AddUserDto AddUser { get; set; }
}
