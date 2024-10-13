using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.Services;

public class AccountService : IAccountService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    
    public AccountService(IUserService userService, IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userService = userService;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> SignUp(CreateUserDto? signUpDto)
    {
        try
        {
            var userId = await _userService.Create(signUpDto);

            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<string> SignIn(SignInDto? signInDto)
    {
        var isValidUser = await _userService.IsExist(signInDto);
        
        if (!isValidUser)
        {
            throw new Exception("Not valid user!");
        }
        
        var user = await _userRepository.GetByUsernameAsync(signInDto?.Username);
        
        if (user == null)
        {
            throw new Exception("Not found user");
        }
        
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
}
