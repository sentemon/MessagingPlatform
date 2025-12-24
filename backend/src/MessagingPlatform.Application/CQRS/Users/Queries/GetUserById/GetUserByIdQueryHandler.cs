using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
{
    private readonly IUserRepository _userRepository;
    
    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IResult<User, Error>> Handle(GetUserByIdQuery query)
    {
        var user = await _userRepository.GetByIdAsync(query.Id);

        return Result<User>.Success(user);
    }
}