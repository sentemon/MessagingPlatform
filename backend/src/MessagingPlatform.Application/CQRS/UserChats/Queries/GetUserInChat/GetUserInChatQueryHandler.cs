using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUserInChat;

public class GetUserInChatQueryHandler : ICommandHandler<GetUserInChatQuery, UserChat>
{
    public async Task<IResult<UserChat, Error>> Handle(GetUserInChatQuery command)
    {
        throw new NotImplementedException();
    }
}