using AutoMapper;
using MediatR;
using MessagingPlatform.Application.Common.Models.UserChatDTOs;
using MessagingPlatform.Application.CQRS.UserChats.Commands.AddUserToChat;
using MessagingPlatform.Application.CQRS.UserChats.Commands.RemoveUserFromChat;
using MessagingPlatform.Application.CQRS.UserChats.Commands.UpdateUserPermissions;
using MessagingPlatform.Application.CQRS.UserChats.Queries.GetUserInChat;
using MessagingPlatform.Application.CQRS.UserChats.Queries.GetUsersInChat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Controllers;


// ToDo
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserChatController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet("{chatId:guid}/users")]
    public async Task<IActionResult> GetUsersInChat(Guid chatId)
    {
        var users = await _mediator.Send(new GetUsersInChatQuery(chatId));
        
        return Ok(users);
    }

    [HttpGet("{chatId:guid}/{username}")]
    public async Task<IActionResult> GetUserInChat(Guid chatId, string username)
    {
        var user = await _mediator.Send(new GetUserInChatQuery(chatId));

        return Ok(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddUserToChat([FromBody] AddUserToChatCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    
    [HttpPut("{chatId:guid}/users/{userId:guid}/permissions")]
    public async Task<IActionResult> UpdateUserPermissions(Guid chatId, Guid userId, [FromBody] UpdateUserPermissionsDto dto)
    {
        var result = await _mediator.Send(new UpdateUserPermissionsCommand(chatId, userId, dto));
        
        return Ok(result);
    }
    
    [HttpDelete("{chatId:guid}/users/{userId:guid}")]
    public async Task<IActionResult> RemoveUserFromChat(Guid chatId, Guid userId)
    {
        var result = await _mediator.Send(new RemoveUserFromChatCommand(chatId, userId));
        
        return Ok(result);
    }
}