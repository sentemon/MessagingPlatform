using MediatR;
using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.UpdateUser.Id);

        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        
        user.FirstName = request.UpdateUser.FirstName;
        user.LastName = request.UpdateUser.LastName;
        user.Username = request.UpdateUser.Username; 
        user.Email = request.UpdateUser.Email;
        user.Bio = request.UpdateUser.Bio;

        return await _userService.Update(user);
    }
}