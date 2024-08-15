using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatform.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _appDbContext;

    public MessageRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<Message> CreateAsync(Guid senderId, Guid receiverId, string content)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            Sender = null, // ToDo: fix
            ReceiverId = receiverId,
            Receiver = null, // ToDo: fix
            Content = content.Trim(),
            SentAt = DateTime.UtcNow
        };

        _appDbContext.Messages.Add(message);
        await _appDbContext.SaveChangesAsync();

        return message;
    }

    public async Task<IQueryable<Message>> GetAllAsync()
    {
        var messages = await Task.FromResult(_appDbContext.Messages.AsQueryable());

        return messages;
    }

    public async Task<IQueryable<Message>> GetByUsernameAsync(string senderUsername, string receiverUsername)
    {
        var messages = await Task.FromResult(_appDbContext.Messages
            .AsQueryable()
            .Where(m => m.Sender.Username == senderUsername && m.Receiver.Username == receiverUsername));

        return messages;
    }

    public async Task<IQueryable<Message>> GetByContentAsync(string senderUsername, string content)
    {
        var messages = await Task.FromResult(_appDbContext.Messages
            .AsQueryable()
            .Where(m => m.Receiver.Username == senderUsername && m.Content == content));

        return messages;
    }

    public async Task<Message> UpdateAsync(Guid senderId, Guid messageId, Message updatedMessage)
    {
        var message = await _appDbContext.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId && m.SenderId == senderId);

        if (message is null)
        {
            throw new KeyNotFoundException("Message not found or user does not have permission to update this message.");
        }
        
        message.Content = updatedMessage.Content;
        message.UpdatedAt = DateTime.UtcNow;
        
        _appDbContext.Messages.Update(message);
        await _appDbContext.SaveChangesAsync();

        return message;
    }


    public async Task<bool> DeleteMessage(Guid senderId, Guid messageId)
    {
        var message = await _appDbContext.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId && m.SenderId == senderId);

        if (message is null)
        {
            return false;
        }
        
        _appDbContext.Messages.Remove(message);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}