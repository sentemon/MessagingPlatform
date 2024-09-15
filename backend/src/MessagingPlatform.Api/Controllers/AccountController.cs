using System.Security.Claims;
using AutoMapper;
using MediatR;
using MessagingPlatform.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Application.CQRS.Users.Commands.AddUser;
using MessagingPlatform.Application.CQRS.Users.Commands.DeleteUser;
using MessagingPlatform.Application.CQRS.Users.Commands.SignIn;
using MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;
using MessagingPlatform.Application.CQRS.Users.Queries.GetAllUsers;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserById;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserByUsername;
using MessagingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace MessagingPlatform.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICookieService _cookieService;
    private readonly IMapper _mapper;

    public AccountController(IMediator mediator, ICookieService cookieService, IMapper mapper)
    {
        _mediator = mediator;
        _cookieService = cookieService;
        _mapper = mapper;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll() // ToDo: only for admins 
    {
        var users = await _mediator.Send(new GetAllUsersQuery());

        return Ok(users.Select(user => _mapper.Map<UserDto>(user)));
    }
    
    // Get Current User
    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        var id = Guid.Parse(currentUserId!);
        
        var user = await _mediator.Send(new GetUserByIdQuery(id));

        return Ok(_mapper.Map<UserDto>(user));
    }
    
    [AllowAnonymous]
    [HttpGet("getbyusername/{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _mediator.Send(new GetUserByUsenameQuery(username));
        
        if (user == null)
        {
            return NotFound("User not found");
        }
        
        return Ok(_mapper.Map<UserDto>(user));
    }
    
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] AddUserDto? signUpDto)
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

        _cookieService.Append("token", token);
        
        return Ok(new { message = "User signed up successfully." });
    }
    
    [AllowAnonymous]
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
        
        _cookieService.Append("token", token);
        
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
    
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto? updateUserDto)
    {
        try
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if (currentUserId == null)
            {
                return NotFound("User id not found");
            }

            var user = _mapper.Map<User>(updateUserDto);
            user.Id = Guid.Parse(currentUserId);
            
            var result = await _mediator.Send(new UpdateUserCommand(user));

            if (!result)
            {
                return NotFound("User not found.");
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
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        
        try
        {
            var guidId = Guid.Parse(currentUserId!);
            
            var result = await _mediator.Send(new DeleteUserCommand(guidId));

            if (!result)
            {
                return NotFound("User not found.");
            }

            return Ok(new { message = "User deleted successfully." });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}