using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Application.CQRS.Users.Commands.AddUser;
using MessagingPlatform.Application.CQRS.Users.Commands.DeleteUser;
using MessagingPlatform.Application.CQRS.Users.Commands.SignIn;
using MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;
using MessagingPlatform.Application.CQRS.Users.Queries.GetAllUsers;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserById;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserByUsername;
using MessagingPlatform.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MessagingPlatform.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly ICookieService _cookieService;
    private readonly GetAllUsersQueryHandler _getAllUsersQueryHandler;
    private readonly GetUserByIdQueryHandler _getUserByIdQueryHandler;
    private readonly GetUserByUsernameQueryHandler _getUserByUsernameQueryHandler;
    private readonly AddUserCommandHandler _addUserCommandHandler;
    private readonly UpdateUserCommandHandler _updateUserCommandHandler;
    private readonly SignInCommandHandler _signInCommandHandler;
    private readonly DeleteUserCommandHandler _deleteUserCommandHandler;
    

    public AccountController(ICookieService cookieService, GetAllUsersQueryHandler getAllUsersQueryHandler, GetUserByIdQueryHandler getUserByIdQueryHandler, GetUserByUsernameQueryHandler getUserByUsernameQueryHandler, AddUserCommandHandler addUserCommandHandler, UpdateUserCommandHandler updateUserCommandHandler, SignInCommandHandler signInCommandHandler, DeleteUserCommandHandler deleteUserCommandHandler)
    {
        _cookieService = cookieService;
        _getAllUsersQueryHandler = getAllUsersQueryHandler;
        _getUserByIdQueryHandler = getUserByIdQueryHandler;
        _getUserByUsernameQueryHandler = getUserByUsernameQueryHandler;
        _addUserCommandHandler = addUserCommandHandler;
        _updateUserCommandHandler = updateUserCommandHandler;
        _signInCommandHandler = signInCommandHandler;
        _deleteUserCommandHandler = deleteUserCommandHandler;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAll() // ToDo: only for admins 
    {
        var query = new GetAllUsersQuery();
        var result = await _getAllUsersQueryHandler.Handle(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Response);
    }
    
    // Get Current GetUser
    [HttpGet("me")]
    public async Task<IActionResult> Get()
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        var id = Guid.Parse(currentUserId);

        var query = new GetUserByIdQuery(id);
        var result = await _getUserByIdQueryHandler.Handle(query);
        
        if (!result.IsSuccess)
        {
            return NotFound(result.Error.Message);
        }

        return Ok(result.Response);
    }
    
    [AllowAnonymous]
    [HttpGet("users/{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var query = new GetUserByUsernameQuery(username.ToLowerInvariant());
        var result = await _getUserByUsernameQueryHandler.Handle(query);
        
        if (!result.IsSuccess)
        {
            return NotFound(result.Error.Message);
        }
        
        return Ok(result.Response);
    }
    
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] AddUserCommand request)
    {
        var command = new AddUserCommand(request.FirstName, request.LastName, request.Username.ToLowerInvariant(), request.Email, request.Password, request.ConfirmPassword);
        var result = await _addUserCommandHandler.Handle(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }

        _cookieService.Append("token", result.Response);
        
        return Ok(new { message = "User signed up successfully." });
    }
    
    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand request)
    {
        var command = new SignInCommand(request.Username.ToLowerInvariant(), request.Password);
        var result = await _signInCommandHandler.Handle(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        _cookieService.Append("token", result.Response);
        
        return Ok(new { message = "You signed in successfully." });
    }
    
    [HttpPost("signout")]
    public new IActionResult SignOut()
    {
        _cookieService.Delete("token");
        
        return Ok(new { message = "User signed out successfully." });
    }
    
    [HttpGet("isauthenticated")]
    public IActionResult IsAuthenticated()
    {
        return Ok(User.Identity?.IsAuthenticated);
    }
    
    [HttpPut("me")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
    {
        try
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if (currentUserId == null)
            {
                return NotFound("User id not found");
            }
            
            var id = Guid.Parse(currentUserId);
            var command = new UpdateUserCommand(id, dto.FirstName, dto.LastName, dto.Email, dto.Bio);
            var result = await _updateUserCommandHandler.Handle(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error.Message);
            }

            return Ok( new { message = "User data updated successfully." });
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
    
    [HttpDelete("me")]
    public async Task<IActionResult> Delete()
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        
        try
        {
            var guidId = Guid.Parse(currentUserId);

            var command = new DeleteUserCommand(guidId);
            var result = await _deleteUserCommandHandler.Handle(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error.Message);
            }
            
            _cookieService.Delete("token");
            
            return Ok(new { message = "User deleted successfully." });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}