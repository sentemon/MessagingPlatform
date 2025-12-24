using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;

public class GetChatsQueryHandler : IQueryHandler<GetChatsQuery, IEnumerable<GetChatSidebarDto>>
{
    private readonly IChatRepository _chatRepository;

    public GetChatsQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IResult<IEnumerable<GetChatSidebarDto>, Error>> Handle(GetChatsQuery query)
    {
        var chats = await _chatRepository.GetAllAsync(query.UserId);

        var chatsDto = new List<GetChatSidebarDto>();

        foreach (var chat in chats)
        {
            if (chat == null)
            {
                continue;
            }

            var lastMessage = chat.Messages?.MaxBy(m => m.SentAt);
            
            var lastMessageFrom = lastMessage?.Sender.Username ?? string.Empty;
            var lastMessageContent = lastMessage?.Content ?? "Start The Conversation";
            var lastMessageSentAt = lastMessage?.SentAt;
            
            var unreadMessagesCount = chat.Messages?
                .Count(m => m.IsRead == false && m.SenderId != query.UserId) ?? 0;

            var chatDto = new GetChatSidebarDto
            {
                ChatId = chat.Id,
                Title = chat.Title,
                LastMessageFrom = lastMessageFrom,
                LastMessageContent = lastMessageContent,
                LastMessageSentAt = lastMessageSentAt,
                UnreadMessagesCount = unreadMessagesCount
            };

            chatsDto.Add(chatDto);
        }
        
        return Result<IEnumerable<GetChatSidebarDto>>.Success(chatsDto);
    }
}