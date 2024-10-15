using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUsersInChat;

public class GetUsersInChatQueryHandler : IRequestHandler<GetUsersInChatQuery, ICollection<UserChat>>
{
    public async Task<ICollection<UserChat>> Handle(GetUsersInChatQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}