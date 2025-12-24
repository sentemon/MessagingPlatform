using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;

public class UpdateChatCommandHandler : ICommandHandler<UpdateChatCommand, bool>
{
    private readonly IChatRepository _chatRepository;

    public UpdateChatCommandHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IResult<bool, Error>> Handle(UpdateChatCommand command)
    {
        var chat = await _chatRepository.GetByIdAsync(command.ChatId);

        if (chat == null)
        {
            return Result<bool>.Failure(new Error("Chat not found"));
        }
        
        var canUserChangeChat = chat.UserChats != null 
                                && chat.UserChats.Any(uc => uc.UserId == command.UserId 
                                                            && uc.Role is ChatRole.Admin or ChatRole.Owner);
        
        if (!canUserChangeChat)
        {
            return Result<bool>.Failure(new Error("User does not have permission to update the chat"));
        }

        chat.Title = command.Title;
        var result = await _chatRepository.UpdateAsync(chat);

        return Result<bool>.Success(result);
    }
}