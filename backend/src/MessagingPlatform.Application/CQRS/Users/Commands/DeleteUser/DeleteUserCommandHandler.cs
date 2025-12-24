using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    
    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IResult<bool, Error>> Handle(DeleteUserCommand command)
    {
        var result = await _userRepository.DeleteAsync(command.Id);

        return Result<bool>.Success(result);
    }
}