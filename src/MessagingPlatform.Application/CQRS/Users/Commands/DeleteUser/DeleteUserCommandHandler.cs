using MediatR;
using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IAccountService _accountService;
    
    public DeleteUserCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _accountService.SignOut();
    }
}