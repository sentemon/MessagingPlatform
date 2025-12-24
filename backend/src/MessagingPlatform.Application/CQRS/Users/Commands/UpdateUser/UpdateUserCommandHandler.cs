using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Infrastructure.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, bool>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    

    public async Task<IResult<bool, Error>> Handle(UpdateUserCommand command)
    {
        var result = await _userService.Update(command.Id, command.FirstName, command.LastName, command.Email, command.Bio);
        
        return Result<bool>.Success(result);
    }
}