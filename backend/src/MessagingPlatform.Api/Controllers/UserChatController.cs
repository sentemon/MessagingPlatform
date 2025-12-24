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
[Route("api/chats/{chatId:guid}/participants")]
[ApiController]
public class UserChatController : ControllerBase
{
    private readonly GetUsersInChatQueryHandler _getUsersInChatQueryHandler;
    private readonly GetUserInChatQueryHandler _getUserInChatQueryHandler;
    private readonly AddUserToChatCommandHandler _addUserToChatCommandHandler;
    private readonly UpdateUserPermissionsCommandHandler _updateUserPermissionsCommandHandler;
    private readonly RemoveUserFromChatCommandHandler _removeUserFromChatCommandHandler;
    
    public UserChatController(GetUsersInChatQueryHandler getUsersInChatQueryHandler, GetUserInChatQueryHandler getUserInChatQueryHandler, AddUserToChatCommandHandler addUserToChatCommandHandler, UpdateUserPermissionsCommandHandler updateUserPermissionsCommandHandler, RemoveUserFromChatCommandHandler removeUserFromChatCommandHandler)
    {
        _getUsersInChatQueryHandler = getUsersInChatQueryHandler;
        _getUserInChatQueryHandler = getUserInChatQueryHandler;
        _addUserToChatCommandHandler = addUserToChatCommandHandler;
        _updateUserPermissionsCommandHandler = updateUserPermissionsCommandHandler;
        _removeUserFromChatCommandHandler = removeUserFromChatCommandHandler;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserChat(Guid chatId)
    {
        var query = new GetUsersInChatQuery(chatId);
        var result = await _getUsersInChatQueryHandler.Handle(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Response);
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserChats(Guid chatId, string username)
    {
        var query = new GetUserInChatQuery(chatId, username);
        var result = await _getUserInChatQueryHandler.Handle(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Response);
    }
    
    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> AddUserChat(Guid chatId, Guid userId)
    {
        var command = new AddUserToChatCommand(chatId, userId);
        var result = await _addUserToChatCommandHandler.Handle(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Response);
    }
    
    [HttpPut("{userId:guid}/permissions")]
    public async Task<IActionResult> UpdateUserPermissions(Guid chatId, Guid userId, [FromBody] UpdateUserPermissionsDto dto)
    {
        var command = new UpdateUserPermissionsCommand(chatId, userId, dto);
        var result = await _updateUserPermissionsCommandHandler.Handle(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Response);
    }
    
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> RemoveUserChat(Guid chatId, Guid userId)
    {
        var command = new RemoveUserFromChatCommand(chatId, userId);
        var result = await _removeUserFromChatCommandHandler.Handle(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Response);
    }
}