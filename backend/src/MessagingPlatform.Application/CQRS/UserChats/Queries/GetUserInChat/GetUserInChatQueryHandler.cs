using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUserInChat;

public class GetUserInChatQueryHandler : ICommandHandler<GetUserInChatQuery, UserChat>
{
    private readonly IChatRepository _chatRepository;

    public GetUserInChatQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IResult<UserChat, Error>> Handle(GetUserInChatQuery command)
    {
        var chat = await _chatRepository.GetByIdAsync(command.ChatId);

        if (chat == null)
        {
            return Result<UserChat>.Failure(new Error("Chat not found"));
        }

        var participant = chat.UserChats?.FirstOrDefault(uc => uc.User.Username == command.Username);

        if (participant == null)
        {
            return Result<UserChat>.Failure(new Error("User not in chat"));
        }

        return Result<UserChat>.Success(participant);
    }
}