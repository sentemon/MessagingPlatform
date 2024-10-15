using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUserInChat;

public class GetUserInChatQueryHandler : IRequestHandler<GetUserInChatQuery, UserChat>
{
    public async Task<UserChat> Handle(GetUserInChatQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}