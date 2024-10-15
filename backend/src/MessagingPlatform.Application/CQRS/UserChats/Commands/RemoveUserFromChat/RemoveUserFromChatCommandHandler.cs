using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.RemoveUserFromChat;

public class RemoveUserFromChatCommandHandler : IRequestHandler<RemoveUserFromChatCommand, UserChat>
{
    public async Task<UserChat> Handle(RemoveUserFromChatCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}