using MediatR;
using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, string?>
{
    private readonly IAccountService _accountService;

    public SignInCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<string?> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        return await _accountService.SignIn(request.SignInDto);
    }
}
