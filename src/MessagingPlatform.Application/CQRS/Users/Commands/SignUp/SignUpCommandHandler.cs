using MediatR;
using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, bool>
{
    private readonly IAccountService _accountService;

    public SignUpCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<bool> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        return await _accountService.SignUp(request.SignUpDto);
    }
}