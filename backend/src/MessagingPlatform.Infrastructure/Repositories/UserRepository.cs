using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MessagingPlatform.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
    {
        return await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
    }
    
    public async Task<IQueryable<User?>> GetAllAsync()
    {
        var users = await Task.FromResult(_context.Users.AsQueryable());

        return users;
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        return user;
    }
    
    public async Task<User?> GetByUsernameAsync(string? username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        return user;
    }

    public async Task AddAsync(User user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task UpdateAsync(User user)
    {
        try
        {
            EntityEntry entityEntry = _context.Entry(user);
            entityEntry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(Guid? id)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Id == id);
            
            if (user == null)
            {
                return false;
            }

            EntityEntry entityEntry = _context.Entry(user);
            entityEntry.State = EntityState.Deleted;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}