using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById;

public class GetChatByIdQueryHandler : IQueryHandler<GetChatByIdQuery, Chat?>
{
    private readonly IChatRepository _chatRepository;

    public GetChatByIdQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IResult<Chat?, Error>> Handle(GetChatByIdQuery query)
    {
        var chat = await _chatRepository.GetByIdAsync(query.ChatId);

        if (chat == null)
        {
            return Result<Chat?>.Failure(new Error("Chat not found"));
        }
        
        var isUserInChat = chat.UserChats != null && chat.UserChats.Any(uc => uc.UserId == query.UserId);
        var result = isUserInChat ? chat : null;
        
        return Result<Chat?>.Success(result);
    }
}