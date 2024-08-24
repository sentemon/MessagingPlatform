using MediatR;
using Microsoft.AspNetCore.Mvc;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Application.CQRS.Users.Commands.AddUser;
using MessagingPlatform.Application.CQRS.Users.Commands.DeleteUser;
using MessagingPlatform.Application.CQRS.Users.Commands.SignIn;
using MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;
using MessagingPlatform.Application.CQRS.Users.Queries.GetAllUsers;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserById;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserByUsername;

namespace MessagingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    private IHttpContextAccessor _httpContextAccessor;

    public AccountController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll() // ToDo: only for admins 
    {
        var users = await _mediator.Send(new GetAllUsersQuery());

        return Ok(users);
    }
    
    [HttpGet("getbyid")]
    public async Task<IActionResult> GetById(Guid id) // ToDo: only for admins
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));

        return Ok(user);
    }
    
    [HttpGet("getbyusername")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _mediator.Send(new GetUserByUsenameQuery(username));

        return Ok(user);
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] AddUserDto? signUpDto) // ToDo: should return token
    {
        if (signUpDto == null)
        {
            return BadRequest("Invalid user data.");
        }

        var token = await _mediator.Send(new AddUserCommand(signUpDto));

        if (token == null)
        {
            return BadRequest("Failed to register user.");
        }

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("token", token);
        
        return Ok("User signed up successfully.");
    }
    
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
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("token", token);
        
        return Ok("You signed in successfully.");
    }
    
    [HttpPost("signout")]
    public new async Task<IActionResult> SignOut()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token");
        return Ok("User signed out successfully.");
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto? updateUserDto) // ToDo: only for the owner of this account
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
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] Guid? id) // ToDo: only for the owner of this account
    {
        if (id is null)
        {
            return BadRequest("Invalid user data.");
        }

        try
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));

            if (!result)
            {
                return NotFound("User not found.");
            }

            return Ok("User deleted successfully.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}