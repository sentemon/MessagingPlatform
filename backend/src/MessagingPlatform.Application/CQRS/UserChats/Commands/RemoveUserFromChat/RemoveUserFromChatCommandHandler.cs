using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.RemoveUserFromChat;

public class RemoveUserFromChatCommandHandler : ICommandHandler<RemoveUserFromChatCommand, UserChat>
{
    public async Task<IResult<UserChat, Error>> Handle(RemoveUserFromChatCommand command)
    {
        throw new NotImplementedException();
    }
}