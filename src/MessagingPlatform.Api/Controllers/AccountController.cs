using Microsoft.AspNetCore.Mvc;
using MessagingPlatform.Application.Common.Models;
using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // POST: api/account/signup
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto? signUpDto)
    {
        if (signUpDto == null)
        {
            return BadRequest("Invalid user data.");
        }

        var result = await _accountService.SignUp(signUpDto);

        if (result)
        {
            return Ok("User registered successfully.");
        }

        return BadRequest("Failed to register user.");
    }

    // POST: api/account/signin
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto? signInDto)
    {
        if (signInDto == null)
        {
            return BadRequest("Invalid user data.");
        }

        var result = await _accountService.SignIn(signInDto);

        if (result)
        {
            return Ok("User signed in successfully.");
        }

        return Unauthorized("Invalid username or password.");
    }

    // POST: api/account/signout
    [HttpPost("signout")]
    public new async Task<IActionResult> SignOut()
    {
        await _accountService.SignOut();
        return Ok("User signed out successfully.");
    }
}