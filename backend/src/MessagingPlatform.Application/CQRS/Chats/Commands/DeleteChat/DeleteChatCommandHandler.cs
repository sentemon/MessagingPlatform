using MediatR;
using MessagingPlatform.Domain.Enums;
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
        var chat = await _chatRepository.GetByIdAsync(request.ChatId);
        
        if (chat == null)
        {
            return false;
        }
        
        var isUserOwnerInChat = chat.UserChats != null 
                                && chat.UserChats.Any(uc => uc.UserId == request.UserId 
                                && uc.Role == ChatRole.Owner);
        
        if (!isUserOwnerInChat)
        {
            return false;
        }
        
        var result = await _chatRepository.DeleteAsync(request.ChatId);

        return result;
    }
}