using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IUserRepository
{
    public Task<IQueryable<User>> GetAllAsync();
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<User?> GetByUsernameAsync(string username);
    public Task AddAsync(User entity, CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, User entity, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}