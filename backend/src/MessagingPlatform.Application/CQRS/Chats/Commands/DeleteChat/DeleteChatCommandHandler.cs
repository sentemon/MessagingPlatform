using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;

public class DeleteChatCommandHandler : ICommandHandler<DeleteChatCommand, bool>
{
    private readonly IChatRepository _chatRepository;

    public DeleteChatCommandHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IResult<bool, Error>> Handle(DeleteChatCommand command)
    {
        var chat = await _chatRepository.GetByIdAsync(command.ChatId);
        
        if (chat == null)
        {
            return Result<bool>.Failure(new Error("Chat not found"));
        }
        
        var isUserOwnerInChat = chat.UserChats != null 
                                && chat.UserChats.Any(uc => uc.UserId == command.UserId 
                                                            && uc.Role == ChatRole.Owner);
        
        if (!isUserOwnerInChat)
        {
            return Result<bool>.Failure(new Error("User is not the owner of the chat"));
        }
        
        var result = await _chatRepository.DeleteAsync(command.ChatId);

        return Result<bool>.Success(result);
    }
}