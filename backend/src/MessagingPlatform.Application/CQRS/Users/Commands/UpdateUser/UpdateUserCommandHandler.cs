using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Infrastructure.Interfaces;
using MessagingPlatform.Domain.Primitives;

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
        try
        {
            var result = await _userService.Update(command.Id, command.FirstName, command.LastName, command.Email, command.Bio);
            return Result<bool>.Success(result);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Failure(new Error(ex.Message));
        }
    }
}