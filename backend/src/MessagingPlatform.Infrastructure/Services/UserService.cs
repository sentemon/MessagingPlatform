using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Infrastructure.Interfaces;

namespace MessagingPlatform.Infrastructure.Services;

public class UserService : IUserService  // ToDo: fix this (перенести всю логику в AccountService)
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Create(string firstName, string lastName, string username, string email, string password, string confirmPassword)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(username);

        if (existingUser != null)
        {
            throw new InvalidOperationException("GetUser with this username already exists.");
        }
        
        var hashedPassword = _passwordHasher.Hash(password);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Username = username,
            Email = email,
            AccountCreatedAt = DateTime.UtcNow,
            PasswordHash = hashedPassword
        };

        await _userRepository.AddAsync(newUser);

        return newUser.Id;
    }

    public async Task<bool> IsExist(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null)
        {
            return false;
        }

        var isPasswordValid = _passwordHasher.Verify(password, user.PasswordHash);

        return isPasswordValid;
    }
    
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    // ToDo: fix logic
    public async Task<bool> Update(Guid id, string firstName, string lastName, string email, string bio)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);

        if (existingUser == null)
        {
            return false;
        }

        existingUser.FirstName = firstName;
        existingUser.LastName = lastName;
        existingUser.Email = email;
        existingUser.Bio = bio;

        await _userRepository.UpdateAsync(existingUser);

        return true;
    }
}
