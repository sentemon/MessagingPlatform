using MediatR;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;

public class UpdateChatCommandHandler : IRequestHandler<UpdateChatCommand, bool>
{
    private readonly IChatRepository _chatRepository;

    public UpdateChatCommandHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    
    public async Task<bool> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.GetByIdAsync(request.UpdateChat.ChatId);

        if (chat == null)
        {
            return false;
        }
        
        var isUserInChat = chat.UserChats != null && chat.UserChats.Any(uc => uc.UserId == request.UserId);
        
        if (!isUserInChat)
        {
            return false;
        }

        chat.Title = request.UpdateChat.Title;
        var result = await _chatRepository.UpdateAsync(chat);

        return result;
    }
}