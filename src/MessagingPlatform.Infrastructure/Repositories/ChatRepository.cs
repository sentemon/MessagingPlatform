using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;
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

    public async Task<Chat> CreateChatAsync(Guid creatorId, List<Guid> userIds, ChatType chatType, string? title = null)
    {
        // Проверка на существование чата
        var existingChat = await _appDbContext.Chats
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c =>
                c.ChatType == chatType &&
                ((chatType == ChatType.Private && c.Users.Any(u => userIds.Contains(u.Id))) ||
                 (chatType == ChatType.Group && c.Title == title)));

        if (existingChat != null)
        {
            return existingChat;
        }

        var users = await _appDbContext.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

        if (users.Count != userIds.Count)
        {
            throw new ArgumentException("Some users not found.");
        }

        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            ChatType = chatType,
            Users = users,
            CreatorId = creatorId,
            Creator = await _appDbContext.Users.FindAsync(creatorId),
            Title = title
        };

        _appDbContext.Chats.Add(chat);
        await _appDbContext.SaveChangesAsync();

        return chat;
    }

    public async Task<Chat> GetChatByIdAsync(Guid chatId)
    {
        return await _appDbContext.Chats
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == chatId);
    }
}