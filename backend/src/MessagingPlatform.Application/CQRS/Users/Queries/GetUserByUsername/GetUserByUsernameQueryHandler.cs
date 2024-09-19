using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, User?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByUsernameQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User?> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        return user;
    }
}