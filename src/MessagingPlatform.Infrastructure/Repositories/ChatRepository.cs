using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatform.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _appDbContext;

    public ChatRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Chat> CreateAsync(Chat chat)
    {
        _appDbContext.Chats.Add(chat);
        await _appDbContext.SaveChangesAsync();
        return chat;
    }

    public async Task<Chat?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Chats
            .Include(c => c.UserChats)!
            .ThenInclude(uc => uc.User)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Chat?>> GetAllAsync()
    {
        return await _appDbContext.Chats
            .Include(c => c.UserChats)!
            .ThenInclude(uc => uc.User)
            .Include(c => c.Messages)
            .ToListAsync();
    }
}