using MediatR;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;

public class UpdateChatCommandHandler : IRequestHandler<UpdateChatCommand, string>
{
    private readonly IChatRepository _chatRepository;

    public UpdateChatCommandHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    
    public async Task<string> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}