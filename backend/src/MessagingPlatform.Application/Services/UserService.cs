using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.Services;

public class UserService : IUserService  // ToDo: fix this (перенести всю логику в AccountService)
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Create(CreateUserDto? signUpDto)
    {
        if (signUpDto == null)
        {
            throw new ArgumentNullException(nameof(signUpDto));
        }

        var existingUser = await _userRepository.GetByUsernameAsync(signUpDto.Username);

        if (existingUser != null)
        {
            throw new InvalidOperationException("GetUser with this username already exists.");
        }
        
        var hashedPassword = _passwordHasher.Hash(signUpDto.Password);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = signUpDto.FirstName,
            LastName = signUpDto.LastName,
            Username = signUpDto.Username,
            Email = signUpDto.Email,
            AccountCreatedAt = DateTime.UtcNow,
            PasswordHash = hashedPassword
        };

        await _userRepository.AddAsync(newUser);

        return newUser.Id;
    }

    public async Task<bool> IsExist(SignInDto? signInDto)
    {
        if (signInDto is null)
        {
            throw new ArgumentNullException(nameof(signInDto));
        }
        
        var user = await _userRepository.GetByUsernameAsync(signInDto.Username);

        if (user == null)
        {
            return false;
        }

        var isPasswordValid = _passwordHasher.Verify(signInDto.Password, user.PasswordHash);

        return isPasswordValid;
    }
    
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }
    
    // ToDo: fix logic
    public async Task<bool> Update(User user)
    {
        var existingUser = await _userRepository.GetByIdAsync(user.Id);

        if (existingUser == null)
        {
            return false;
        }

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.Bio = user.Bio;
        // existingUser.Username = user.Username;

        await _userRepository.UpdateAsync(existingUser);

        return true;
    }
}
