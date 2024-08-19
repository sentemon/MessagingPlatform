using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IQueryable<User>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IQueryable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();

        return users;
    }
}