using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.AddUserToChat;

public class AddUserToChatCommandHandler : IRequestHandler<AddUserToChatCommand, UserChat>
{
    public async Task<UserChat> Handle(AddUserToChatCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}