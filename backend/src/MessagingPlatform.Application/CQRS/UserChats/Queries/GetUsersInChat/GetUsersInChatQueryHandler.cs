using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.UserChats.Queries.GetUsersInChat;

public class GetUsersInChatQueryHandler : IQueryHandler<GetUsersInChatQuery, ICollection<UserChat>>
{
    private readonly IChatRepository _chatRepository;

    public GetUsersInChatQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IResult<ICollection<UserChat>, Error>> Handle(GetUsersInChatQuery command)
    {
        var chat = await _chatRepository.GetByIdAsync(command.ChatId);

        if (chat == null)
        {
            return Result<ICollection<UserChat>>.Failure(new Error("Chat not found"));
        }

        var participants = chat.UserChats?.ToList() ?? new List<UserChat>();
        return Result<ICollection<UserChat>>.Success(participants);
    }
}