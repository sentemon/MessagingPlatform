using MediatR;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;

public class DeleteChatCommandHandler : IRequestHandler<DeleteChatCommand, bool>
{
    private readonly IChatRepository _chatRepository;

    public DeleteChatCommandHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    
    public async Task<bool> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}