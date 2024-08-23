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

    public async Task<bool> SignUp(AddUserDto? signUpDto)
    {
        try
        {
            var userId = await _userService.Create(signUpDto);
            
            return userId != Guid.Empty;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
            return false;
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

    public async Task SignOut() // ToDo: fix
    {
        await Task.CompletedTask;
    }
}
