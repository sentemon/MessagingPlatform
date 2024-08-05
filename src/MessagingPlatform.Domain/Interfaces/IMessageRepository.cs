using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IMessageRepository
{
    Task<Message> CreateAsync(User sender, User receiver, string content);
    Task<IQueryable<Message>>? GetAllAsync();
    Task<IQueryable<Message>>? GetByUsernameAsync(string senderUsername, string receiverUsername);
    Task<IQueryable<Message>>? GetByContentAsync(string senderUsername, string content);
}