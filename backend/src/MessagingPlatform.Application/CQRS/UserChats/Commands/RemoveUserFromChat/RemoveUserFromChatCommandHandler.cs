using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Domain.Primitives;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.RemoveUserFromChat;

public class RemoveUserFromChatCommandHandler : ICommandHandler<RemoveUserFromChatCommand, UserChat>
{
    private readonly IChatRepository _chatRepository;

    public RemoveUserFromChatCommandHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IResult<UserChat, Error>> Handle(RemoveUserFromChatCommand command)
    {
        var chat = await _chatRepository.GetByIdAsync(command.ChatId);
        if (chat == null)
        {
            return Result<UserChat>.Failure(new Error("Chat not found"));
        }

        var participant = chat.GetParticipant(command.UserId);
        if (participant == null)
        {
            return Result<UserChat>.Failure(new Error("User not in chat"));
        }

        try
        {
            var removed = chat.RemoveParticipant(command.UserId);
            if (!removed)
            {
                return Result<UserChat>.Failure(new Error("Cannot remove user from chat"));
            }

            await _chatRepository.UpdateAsync(chat);
            return Result<UserChat>.Success(participant);
        }
        catch (DomainException ex)
        {
            return Result<UserChat>.Failure(new Error(ex.Message));
        }
    }
}