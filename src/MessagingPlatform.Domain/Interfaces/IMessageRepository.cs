using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IMessageRepository
{
    Task<Message> CreateAsync(Guid senderId, Guid receiverId, string content);
    Task<IQueryable<Message>> GetAllAsync();
    Task<IQueryable<Message>> GetByUsernameAsync(string senderUsername, string receiverUsername);
    Task<IQueryable<Message>> GetByContentAsync(string senderUsername, string content);
    Task<Message> UpdateAsync(Guid senderId, Guid messageId, Message updatedMessage);
    Task<bool> DeleteMessage(Guid senderId, Guid messageId);
}