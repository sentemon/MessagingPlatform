using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUsersInChat;

public class GetUsersInChatQueryHandler : IQueryHandler<GetUsersInChatQuery, ICollection<UserChat>>
{
    public async Task<IResult<ICollection<UserChat>, Error>> Handle(GetUsersInChatQuery command)
    {
        throw new NotImplementedException();
    }
}