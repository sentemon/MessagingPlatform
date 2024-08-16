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

    public async Task<Message> CreateAsync(Guid senderId, Guid chatId, string content)
    {
        var sender = await _appDbContext.Users.FindAsync(senderId);
        var chat = await _appDbContext.Chats.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == chatId);

        if (sender == null || chat == null || chat.Users.All(u => u.Id != senderId))
        {
            throw new ArgumentException("Invalid sender or chat.");
        }

        var message = new Message
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            Sender = sender,
            ChatId = chatId,
            Chat = chat,
            Content = content.Trim(),
            SentAt = DateTime.UtcNow
        };

        _appDbContext.Messages.Add(message);
        await _appDbContext.SaveChangesAsync();

        return message;
    }

    public async Task<IQueryable<Message>> GetAllAsync()
    {
        return await Task.FromResult(_appDbContext.Messages.AsQueryable());
    }

    public async Task<IQueryable<Message>> GetByUsernameAsync(string senderUsername, string chatTitle)
    {
        var messages = await Task.FromResult(_appDbContext.Messages
            .Include(m => m.Sender)
            .Include(m => m.Chat)
            .Where(m => m.Sender.Username == senderUsername && m.Chat.Title == chatTitle)
            .AsQueryable());

        return messages;
    }

    public async Task<IQueryable<Message>> GetByContentAsync(string senderUsername, string content)
    {
        var messages = await Task.FromResult(_appDbContext.Messages
            .Include(m => m.Sender)
            .Where(m => m.Sender.Username == senderUsername && m.Content.Contains(content))
            .AsQueryable());

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