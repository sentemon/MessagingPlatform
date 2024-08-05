using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Common.Models;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasherService passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Create(AddUserDto? signUpDto)
    {
        if (signUpDto == null)
        {
            throw new ArgumentNullException(nameof(signUpDto));
        }

        var existingUser = await _userRepository.GetByUsernameAsync(signUpDto.Username);

        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
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
        if (signInDto == null)
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
}
