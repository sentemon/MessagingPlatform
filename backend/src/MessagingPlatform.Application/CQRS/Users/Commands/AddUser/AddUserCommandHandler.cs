using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Infrastructure.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.AddUser;

public class AddUserCommandHandler : ICommandHandler<AddUserCommand, string>
{
    private readonly IAccountService _accountService;

    public AddUserCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<IResult<string, Error>> Handle(AddUserCommand command)
    {
        var token = await _accountService.SignUp(command.FirstName, command.LastName, command.Username, command.Email, command.Password, command.ConfirmPassword);
        
        return Result<string>.Success(token);
    }
}