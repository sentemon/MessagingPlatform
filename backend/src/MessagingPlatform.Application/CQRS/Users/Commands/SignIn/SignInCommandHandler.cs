using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Infrastructure.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.SignIn;

public class SignInCommandHandler : ICommandHandler<SignInCommand, string>
{
    private readonly IAccountService _accountService;

    public SignInCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<IResult<string, Error>> Handle(SignInCommand command)
    {
        var token = await _accountService.SignIn(command.Username, command.Password);
        
        return Result<string>.Success(token);
    }
}
