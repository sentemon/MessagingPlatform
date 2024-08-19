using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IMessageRepository
{
    Task<Message> CreateAsync(Guid senderId, Guid chatId, string content);
    Task<IQueryable<Message>> GetAllAsync();
    Task<IQueryable<Message>> GetByUserIdAndChatId(Guid userId, Guid chatId);
    Task<IQueryable<Message>> GetByUsernameAsync(string senderUsername, string chatTitle);
    Task<IQueryable<Message>> GetByContentAsync(string senderUsername, string content);
    Task<Message> UpdateAsync(Guid senderId, Guid messageId, Message updatedMessage);
    Task<bool> DeleteMessage(Guid senderId, Guid messageId);
}