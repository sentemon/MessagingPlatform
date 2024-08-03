using MediatR;
using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Application.Users.Commands.SignOut;


public class SignOutCommandHandler : IRequestHandler<SignOutCommand>
{
    private readonly IAccountService _accountService;

    public SignOutCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _accountService.SignOut();
    }
}
