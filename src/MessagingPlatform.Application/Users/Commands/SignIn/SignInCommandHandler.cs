using MediatR;
using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Application.Users.Commands.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, bool>
{
    private readonly IAccountService _accountService;

    public SignInCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<bool> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        return await _accountService.SignIn(request.SignInDto);
    }
}
