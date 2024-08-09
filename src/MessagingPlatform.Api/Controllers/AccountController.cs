using MediatR;
using Microsoft.AspNetCore.Mvc;
using MessagingPlatform.Application.Common.Models;
using MessagingPlatform.Application.CQRS.Users.Commands.AddUser;
using MessagingPlatform.Application.CQRS.Users.Commands.SignIn;
using MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;
using Microsoft.AspNetCore.Authorization;

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
    public async Task<IActionResult> SignUp([FromBody] AddUserDto? signUpDto)
    {
        if (signUpDto == null)
        {
            return BadRequest("Invalid user data.");
        }

        var result = await _mediator.Send(new AddUserCommand(signUpDto));

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

        var token = await _mediator.Send(new SignInCommand(signInDto));

        if (token == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(new { Token = token });
    }
    
    // POST: api/account/signout
    [HttpPost("signout")]
    public new async Task<IActionResult> SignOut()
    {
        return Ok("User signed out successfully.");
    }
    
    // POST: api/account/update
    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto? updateUserDto)
    {
        if (updateUserDto == null)
        {
            return BadRequest("Invalid user data.");
        }

        var currentUser = User.Identity?.Name;
    
        if (currentUser == null || !currentUser.Equals(updateUserDto.Username, StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized("You can only update your own profile.");
        }

        try
        {
            var result = await _mediator.Send(new UpdateUserCommand(updateUserDto));

            if (!result)
            {
                return NotFound("User not found.");
            }

            return Ok("User data updated successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}