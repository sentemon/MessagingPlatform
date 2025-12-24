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
    
    public async Task<IQueryable<Message>> GetAllAsync(Guid chatId)
    {
        var messages = _appDbContext.Messages
            .Include(m => m.Sender)
            .Include(m => m.Chat)
            .Where(m => m.ChatId == chatId)
            .AsQueryable();

        return messages;
    }

    public async Task<Message> GetById(Guid id)
    {
        var message = await _appDbContext.Messages.FirstAsync(m => m.Id == id);

        return message;
    }

    public async Task<Message> CreateAsync(Guid senderId, Guid chatId, string content)
    {
        var chat = await _appDbContext.Chats
            .Include(c => c.UserChats)!
                .ThenInclude(uc => uc.User)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat == null)
        {
            throw new ArgumentException("Invalid chat.");
        }

        var sender = chat.UserChats.FirstOrDefault(uc => uc.UserId == senderId)?.User;

        sender ??= await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == senderId);

        if (sender == null)
        {
            throw new ArgumentException("Invalid sender.");
        }

        var message = chat.AddMessage(sender, content, DateTime.UtcNow);

        await _appDbContext.SaveChangesAsync();

        return message;
    }

    public async Task<IQueryable<Message>> GetByContentAsync(string senderUsername, string content)
    {
        var messages = _appDbContext.Messages
            .Include(m => m.Sender)
            .Where(m => m.Sender.Username == senderUsername && m.Content.Contains(content))
            .AsQueryable();

        return messages;
    }

    public async Task<Message> UpdateAsync(Message updatedMessage)
    {
        _appDbContext.Messages.Update(updatedMessage);
        await _appDbContext.SaveChangesAsync();

        return updatedMessage;
    }

    public async Task<bool> DeleteMessage(Guid senderId, Guid messageId)
    {
        var message = await _appDbContext.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId);

        if (message == null)
        {
            return false;
        }

        if (!message.CanBeModifiedBy(senderId))
        {
            return false;
        }

        _appDbContext.Messages.Remove(message);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}
