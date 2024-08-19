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
        var chat = await _appDbContext.Chats
            .Include(c => c.UserChats)!
                .ThenInclude(uc => uc.User)
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat == null || chat.UserChats!.All(uc => uc.UserId != senderId))
        {
            throw new ArgumentException("Invalid sender or chat.");
        }

        var sender = await _appDbContext.Users.FindAsync(senderId);

        if (sender == null)
        {
            throw new ArgumentException("Invalid sender.");
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
        var messages = _appDbContext.Messages
            .Include(m => m.Sender)
            .Include(m => m.Chat)
            .AsQueryable();

        return messages;
    }

    public async Task<IQueryable<Message>> GetByUserIdAndChatId(Guid userId, Guid chatId)  //ToDo: check if user has access to the chat(already done) maybe change in the future
    {
        var hasAccess = await _appDbContext.UserChats
            .AnyAsync(uc => uc.UserId == userId && uc.ChatId == chatId);
        
        if (!hasAccess)
        {
            throw new Exception("you don't have permissions to see it");
        }
        var messages = _appDbContext.Messages
            .Where(m => m.ChatId == chatId)
            .Include(m => m.Sender)
            .Include(m => m.Chat)
            .AsQueryable();

        return messages;
    }
    
    public async Task<IQueryable<Message>> GetByUsernameAsync(string senderUsername, string chatTitle)
    {
        var messages = _appDbContext.Messages
            .Include(m => m.Sender)
            .Include(m => m.Chat)
            .Where(m => m.Sender.Username == senderUsername && m.Chat.Title == chatTitle)
            .AsQueryable();

        return messages;
    }

    public async Task<IQueryable<Message>> GetByContentAsync(string senderUsername, string content)
    {
        var messages = _appDbContext.Messages
            .Include(m => m.Sender)
            .Where(m => m.Sender.Username == senderUsername && m.Content.Contains(content))
            .AsQueryable();

        return messages;
    }

    public async Task<Message> UpdateAsync(Guid senderId, Guid messageId, Message updatedMessage)
    {
        var message = await _appDbContext.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId && m.SenderId == senderId);

        if (message == null)
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

        if (message == null)
        {
            return false;
        }

        _appDbContext.Messages.Remove(message);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}
