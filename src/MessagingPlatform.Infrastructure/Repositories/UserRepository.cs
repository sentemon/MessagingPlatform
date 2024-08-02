using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Infrastructure.Data;
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
    public async Task<IQueryable<User>> GetAllAsync()
    {
        var users = await Task.FromResult(_context.Users.AsQueryable());

        return users;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return user;
    }
    
    public async Task<User?> GetByUsernameAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        return user;
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Users.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task UpdateAsync(Guid id, User entity, CancellationToken cancellationToken)
    {
        try
        {
            EntityEntry entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
            
            if (user == null)
            {
                return false;
            }

            EntityEntry entityEntry = _context.Entry(user);
            entityEntry.State = EntityState.Deleted;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}