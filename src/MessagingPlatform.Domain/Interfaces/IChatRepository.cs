using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IChatRepository
{
    Task<Chat> CreateAsync(Chat chat);
    Task<Chat> GetByIdAsync(Guid id);
    Task<IEnumerable<Chat>> GetAllAsync();
}