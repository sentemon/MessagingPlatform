using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Infrastructure.Interfaces;

namespace MessagingPlatform.Infrastructure.Services;

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

    public async Task<string> SignUp(string firstName, string lastName, string username, string email, string password, string confirmPassword)
    {
        try
        {
            var userId = await _userService.Create(firstName, lastName, username, email, password, confirmPassword);

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

    public async Task<string> SignIn(string username, string password)
    {
        var isValidUser = await _userService.IsExist(username, password);
        
        if (!isValidUser)
        {
            throw new Exception("Not valid user!");
        }
        
        var user = await _userRepository.GetByUsernameAsync(username);
        
        if (user == null)
        {
            throw new Exception("Not found user");
        }
        
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
}
