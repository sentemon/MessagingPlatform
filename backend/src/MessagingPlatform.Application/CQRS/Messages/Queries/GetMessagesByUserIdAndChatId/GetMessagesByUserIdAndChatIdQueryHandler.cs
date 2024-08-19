using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Queries.GetMessagesByUserIdAndChatId;

public class GetMessagesByUserIdAndChatIdQueryHandler : IRequestHandler<GetMessagesByUserIdAndChatIdQuery, IQueryable<Message>>
{
    private readonly IMessageRepository _messageRepository;

    public GetMessagesByUserIdAndChatIdQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public async Task<IQueryable<Message>> Handle(GetMessagesByUserIdAndChatIdQuery request, CancellationToken cancellationToken)
    {
        var messages = await _messageRepository.GetByUserIdAndChatId(request.UserId, request.ChatId);

        return messages;
    }
}