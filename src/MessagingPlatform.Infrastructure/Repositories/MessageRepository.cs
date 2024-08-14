using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Infrastructure.Persistence;

namespace MessagingPlatform.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _appDbContext;

    public MessageRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<Message> CreateAsync(User sender, User receiver, string content)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            SenderId = sender.Id,
            Sender = sender,
            ReceiverId = receiver.Id,
            Receiver = receiver,
            Content = content.Trim(),
            SentAt = DateTime.UtcNow
        };

        _appDbContext.Messages.Add(message);
        await _appDbContext.SaveChangesAsync();

        return message;
    }

    public async Task<IQueryable<Message>>? GetAllAsync()
    {
        var messages = await Task.FromResult(_appDbContext.Messages.AsQueryable());

        return messages;
    }

    public async Task<IQueryable<Message>>? GetByUsernameAsync(string senderUsername, string receiverUsername)
    {
        var messages = await Task.FromResult(_appDbContext.Messages
            .AsQueryable()
            .Where(m => m.Sender.Username == senderUsername && m.Receiver.Username == receiverUsername));

        return messages;
    }

    public async Task<IQueryable<Message>>? GetByContentAsync(string senderUsername, string content)
    {
        var messages = await Task.FromResult(_appDbContext.Messages
            .AsQueryable()
            .Where(m => m.Receiver.Username == senderUsername && m.Content == content));

        return messages;
    }
}