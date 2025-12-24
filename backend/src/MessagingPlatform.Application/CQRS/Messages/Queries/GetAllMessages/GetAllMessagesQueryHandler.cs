using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Queries.GetAllMessages;

public class GetAllMessagesQueryHandler : IQueryHandler<GetAllMessagesQuery, IQueryable<Message>>
{
    private readonly IMessageRepository _messageRepository;

    public GetAllMessagesQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<IResult<IQueryable<Message>, Error>> Handle(GetAllMessagesQuery query)
    {
        var messages = await _messageRepository.GetAllAsync(query.ChatId);

        return Result<IQueryable<Message>>.Success(messages); 
    }
}