using MediatR;
using Microsoft.AspNetCore.Mvc;
using MessagingPlatform.Application.Common.Models;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.CQRS.Users.Commands.SignIn;
using MessagingPlatform.Application.CQRS.Users.Commands.SignOut;
using MessagingPlatform.Application.CQRS.Users.Commands.SignUp;

namespace MessagingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/account/signup
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto? signUpDto)
    {
        if (signUpDto == null)
        {
            return BadRequest("Invalid user data.");
        }

        var result = await _mediator.Send(new SignUpCommand(signUpDto));

        if (!result)
        {
            return BadRequest("Failed to register user.");
        }

        return Ok("User registered successfully.");
    }

    // POST: api/account/signin
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto? signInDto)
    {
        if (signInDto == null)
        {
            return BadRequest("Invalid user data.");
        }

        var result = await _mediator.Send(new SignInCommand(signInDto));

        if (!result)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok("User signed in successfully.");
    }

    // POST: api/account/signout
    [HttpPost("signout")]
    public new async Task<IActionResult> SignOut()
    {
        await _mediator.Send(new SignOutCommand());
        
        return Ok("User signed out successfully.");
    }
}