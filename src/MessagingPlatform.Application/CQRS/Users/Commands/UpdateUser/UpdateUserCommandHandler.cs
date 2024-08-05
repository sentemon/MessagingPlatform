using MediatR;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.UpdateUser == null)
        {
            throw new ArgumentNullException(nameof(request), "Input cannot be null");
        }

        var user = await _userRepository.GetByIdAsync(request.UpdateUser.Id);
        
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.UpdateUser.Id} not found");
        }

        user.FirstName = request.UpdateUser.FirstName;
        user.LastName = request.UpdateUser.LastName;
        user.Username = request.UpdateUser.Username;
        user.Email = request.UpdateUser.Email;
        user.Bio = request.UpdateUser.Bio;
        
        await _userRepository.UpdateAsync(user);
    }
    
}