using MediatR;
using MessagingPlatform.Application.Common.Models;

namespace MessagingPlatform.Application.Users.Commands.SignUp;

public class SignUpCommand : IRequest<bool>
{
    public SignUpCommand(SignUpDto signUpDto)
    {
        SignUpDto = signUpDto;
    }
    
    public SignUpDto SignUpDto { get; set; }
}