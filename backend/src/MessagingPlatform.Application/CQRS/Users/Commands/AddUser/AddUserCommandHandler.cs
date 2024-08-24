using MediatR;
using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.AddUser;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, string?>
{
    private readonly IAccountService _accountService;

    public AddUserCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<string?> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var token = await _accountService.SignUp(request.AddUser);
        
        return token;
    }
}