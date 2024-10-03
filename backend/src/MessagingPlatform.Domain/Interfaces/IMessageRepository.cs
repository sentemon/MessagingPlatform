using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IMessageRepository
{
    Task<IQueryable<Message>> GetAllAsync(Guid chatId);
    Task<Message> CreateAsync(Guid senderId, Guid chatId, string content);
    Task<Message> UpdateAsync(Guid senderId, Guid messageId, Message updatedMessage);
    Task<bool> DeleteMessage(Guid senderId, Guid messageId);
}