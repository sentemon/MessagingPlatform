using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Infrastructure.Interfaces;

public interface IUserService
{
    Task<Guid> Create(string firstName, string lastName, string username, string email, string password, string confirmPassword);
    Task<bool> IsExist(string username, string password);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<bool> Update(Guid id, string firstName, string lastName, string email, string bio);
}