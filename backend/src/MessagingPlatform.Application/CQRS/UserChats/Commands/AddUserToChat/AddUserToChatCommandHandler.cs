using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.AddUserToChat;

public class AddUserToChatCommandHandler : ICommandHandler<AddUserToChatCommand, UserChat>
{
    public async Task<IResult<UserChat, Error>> Handle(AddUserToChatCommand command)
    {
        throw new NotImplementedException();
    }
}