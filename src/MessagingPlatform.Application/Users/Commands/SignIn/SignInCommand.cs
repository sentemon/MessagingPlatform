using MediatR;
using MessagingPlatform.Application.Common.Models;

namespace MessagingPlatform.Application.Users.Commands.SignIn;

public class SignInCommand : IRequest<bool>
{
    public SignInCommand(SignInDto signInDto)
    {
        SignInDto = signInDto;
    }
    
    public SignInDto SignInDto { get; set; }
}
