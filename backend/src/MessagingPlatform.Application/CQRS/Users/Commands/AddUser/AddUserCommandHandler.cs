using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Infrastructure.Interfaces;
using MessagingPlatform.Domain.Primitives;

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
        try
        {
            var token = await _accountService.SignUp(command.FirstName, command.LastName, command.Username, command.Email, command.Password, command.ConfirmPassword);
            return Result<string>.Success(token);
        }
        catch (DomainException ex)
        {
            return Result<string>.Failure(new Error(ex.Message));
        }
    }
}