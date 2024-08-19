using MediatR;
using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.AddUser;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, bool>
{
    private readonly IAccountService _accountService;

    public AddUserCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        return await _accountService.SignUp(request.AddUser);
    }
}