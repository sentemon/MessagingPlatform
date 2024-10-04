using MediatR;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;

public class GetChatsQueryHandler : IRequestHandler<GetChatsQuery, IEnumerable<ChatSidebarDto>>
{
    private readonly IChatRepository _chatRepository;

    public GetChatsQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IEnumerable<ChatSidebarDto>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _chatRepository.GetAllAsync(request.UserId);

        var chatsDto = new List<ChatSidebarDto>();

        foreach (var chat in chats)
        {
            if (chat == null)
            {
                continue;
            }

            var lastMessage = chat.Messages?.MaxBy(m => m.SentAt);
            
            var unreadMessagesCount = chat.Messages?
                .Count(m => !m.IsRead.HasValue || m.IsRead == false && m.SenderId != request.UserId) ?? 0;


            var chatDto = new ChatSidebarDto
            {
                ChatId = chat.Id,
                Title = chat.Title ?? "New Chat",
                LastMessageFrom = lastMessage?.Sender.Username,
                LastMessageContent = lastMessage?.Content ?? "Start The Conversation",
                LastMessageSentAt = lastMessage?.SentAt,
                UnreadMessagesCount = unreadMessagesCount
            };

            chatsDto.Add(chatDto);
        }
    
        return chatsDto;
    }

}