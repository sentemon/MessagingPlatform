using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IGroupRepository
{
    Task<Group> GetByIdAsync(Guid id);
    Task<IEnumerable<Group>> GetAllAsync();
    Task AddAsync(Group group);
    Task UpdateAsync(Group group);
    Task DeleteAsync(Group group);
}