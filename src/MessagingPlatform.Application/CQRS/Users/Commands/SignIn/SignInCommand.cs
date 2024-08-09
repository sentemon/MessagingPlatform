using MediatR;
using MessagingPlatform.Application.Common.Models;

namespace MessagingPlatform.Application.CQRS.Users.Commands.SignIn;

public class SignInCommand : IRequest<string?>
{
    public SignInCommand(SignInDto signInDto)
    {
        SignInDto = signInDto;
    }
    
    public SignInDto SignInDto { get; set; }
}
