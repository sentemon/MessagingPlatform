using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IMessageRepository
{
    Task<IQueryable<Message>> GetAllAsync(Guid chatId);
    Task<Message> GetById(Guid id);
    Task<Message> CreateAsync(Guid senderId, Guid chatId, string content);
    Task<Message> UpdateAsync(Message updatedMessage);
    Task<bool> DeleteMessage(Guid senderId, Guid messageId);
}