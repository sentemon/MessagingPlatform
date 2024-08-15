using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Queries.GetAllMessages;

public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, IQueryable<Message>>
{
    private readonly IMessageRepository _messageRepository;

    public GetAllMessagesQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public async Task<IQueryable<Message>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _messageRepository.GetAllAsync(); // ToDo: change the params

        return messages; 
    }
}