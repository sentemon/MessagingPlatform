using MediatR;
using MessagingPlatform.Application.Common.Models;
using MessagingPlatform.Application.CQRS.Users.Commands.AddUser;
using MessagingPlatform.Application.CQRS.Users.Commands.DeleteUser;
using MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;
using MessagingPlatform.Application.CQRS.Users.Queries.GetAllUsers;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserById;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserByUsername;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());

        return Ok(users);
    }

    [HttpGet("userbyid")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));

        return Ok(user);
    }

    [HttpGet("userbyusername")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _mediator.Send(new GetUserByUsenameQuery(username));

        return Ok(user);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddUserDto? addUserDto)
    {
        if (addUserDto is null)
        {
            return BadRequest("Invalid user data");
        }

        var result = await _mediator.Send(new AddUserCommand(addUserDto));

        return Ok(result);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto? updateUserDto)
    {
        if (updateUserDto is null) // ToDo: to fix
        {
            return BadRequest("Invalid user data.");
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
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] Guid? id)
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
