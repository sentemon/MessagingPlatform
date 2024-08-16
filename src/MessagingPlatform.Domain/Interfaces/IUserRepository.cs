using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds);
    public Task<IQueryable<User>> GetAllAsync();
    public Task<User?> GetByIdAsync(Guid id);
    public Task<User?> GetByUsernameAsync(string? username);
    public Task AddAsync(User user);
    public Task UpdateAsync(User user);
    public Task<bool> DeleteAsync(Guid? id);
}