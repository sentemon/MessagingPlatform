using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;

public class GetChatsQueryHandler : IRequestHandler<GetChatsQuery, IEnumerable<Chat?>>
{
    private readonly IChatRepository _chatRepository;

    public GetChatsQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IEnumerable<Chat?>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _chatRepository.GetAllAsync(request.UserId);
        
        return chats;
    }
}