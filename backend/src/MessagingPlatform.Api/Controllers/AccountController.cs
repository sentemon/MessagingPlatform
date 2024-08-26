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

        return Ok(users);
    }
    
    // Get Current User
    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        var id = Guid.Parse(currentUserId!);
        
        var user = await _mediator.Send(new GetUserByIdQuery(id));

        return Ok(user);
    }
    
    [AllowAnonymous]
    [HttpGet("getbyusername")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _mediator.Send(new GetUserByUsenameQuery(username));
        
        if (user == null)
        {
            return NotFound("User not found");
        }

        var userDto = (user.FirstName, user.LastName, user.Username, user.Bio).ToString();
        
        return Ok(userDto);
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
        
        return Ok("User signed up successfully.");
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
        
        return Ok("You signed in successfully.");
    }
    
    [HttpPost("signout")]
    public new async Task<IActionResult> SignOut()
    {
        _cookieService.Delete("token");
        
        return Ok("User signed out successfully.");
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto? updateUserDto)
    {
        if (updateUserDto!.Username.IsNullOrEmpty())  // ToDo: some date can be null
        {
            return BadRequest("Username cannot be null.");
        }

        try
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if (currentUserId == null)
            {
                return NotFound("User id not found");
            }
            
            updateUserDto.Id = Guid.Parse(currentUserId);
            
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

            return Ok("User deleted successfully.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}