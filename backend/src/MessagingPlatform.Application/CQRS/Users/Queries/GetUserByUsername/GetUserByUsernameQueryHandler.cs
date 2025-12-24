using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQueryHandler : IQueryHandler<GetUserByUsernameQuery, User?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByUsernameQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IResult<User?, Error>> Handle(GetUserByUsernameQuery query)
    {
        var user = await _userRepository.GetByUsernameAsync(query.Username);

        return Result<User?>.Success(user);
    }
}