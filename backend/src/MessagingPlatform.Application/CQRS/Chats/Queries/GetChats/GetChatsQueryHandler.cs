using MediatR;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;

public class GetChatsQueryHandler : IRequestHandler<GetChatsQuery, IEnumerable<GetChatSidebarDto>>
{
    private readonly IChatRepository _chatRepository;

    public GetChatsQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IEnumerable<GetChatSidebarDto>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _chatRepository.GetAllAsync(request.UserId);

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
                .Count(m => m.IsRead == false && m.SenderId != request.UserId) ?? 0;

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
        
        return chatsDto;
    }


}