using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds);
    Task<IQueryable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string? username);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid? id);
}