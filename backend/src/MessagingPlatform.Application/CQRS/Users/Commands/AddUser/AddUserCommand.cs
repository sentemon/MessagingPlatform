using MediatR;
using MessagingPlatform.Application.Common.Models.UserDTOs;

namespace MessagingPlatform.Application.CQRS.Users.Commands.AddUser;

public class AddUserCommand : IRequest<string?>
{
    public AddUserCommand(CreateUserDto createUser)
    {
        CreateUser = createUser;
    }

    public CreateUserDto CreateUser { get; set; }
}
