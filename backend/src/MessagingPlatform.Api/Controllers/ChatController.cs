using System.Security.Claims;
using MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;
using MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;
using MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly GetChatsQueryHandler _getChatsQueryHandler;
    private readonly GetChatByIdQueryHandler _getChatByIdQueryHandler;
    private readonly CreateChatCommandHandler _createChatCommandHandler;
    private readonly UpdateChatCommandHandler _updateChatCommandHandler;
    private readonly DeleteChatCommandHandler _deleteChatCommandHandler;
    
    public ChatController(GetChatsQueryHandler getChatsQueryHandler, GetChatByIdQueryHandler getChatByIdQueryHandler, CreateChatCommandHandler createChatCommandHandler, UpdateChatCommandHandler updateChatCommandHandler, DeleteChatCommandHandler deleteChatCommandHandler)
    {
        _getChatsQueryHandler = getChatsQueryHandler;
        _getChatByIdQueryHandler = getChatByIdQueryHandler;
        _createChatCommandHandler = createChatCommandHandler;
        _updateChatCommandHandler = updateChatCommandHandler;
        _deleteChatCommandHandler = deleteChatCommandHandler;
    }
        
    [HttpGet("getall")]
    public async Task<IActionResult> GetChats()
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);

        var query = new GetChatsQuery(userId);
        var result = await _getChatsQueryHandler.Handle(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }

        return Ok(result.Response);
    }
        
    [HttpGet("get")]
    public async Task<IActionResult> GetChat(Guid id)
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);

        var query = new GetChatByIdQuery(id, userId);
        var result = await _getChatByIdQueryHandler.Handle(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Response);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChatDto)
    {
        var creatorId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        var command = new CreateChatCommand(createChatDto.ChatType, createChatDto.Usernames, creatorId);
        var result = await _createChatCommandHandler.Handle(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }

        return Ok(result.Response.Id); // maybe return the entity of chat
    }
        
    [HttpPut("update")]
    public async Task<IActionResult> UpdateChat(Guid chatId, string title)
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        var command = new UpdateChatCommand(userId, chatId, title);
        var result = await _updateChatCommandHandler.Handle(command);

        if (!result.IsSuccess)
        {
            return  BadRequest(result.Error.Message);
        }
    
        return Ok("Chat updated successfully.");
    }
        
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteChat(Guid id)
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        var command = new DeleteChatCommand(id, userId);
        var result = await _deleteChatCommandHandler.Handle(command);

        if (!result.IsSuccess)
        {
            return BadRequest("Chat not found or you do not have permissions to delete it.");
        }

        return Ok("Chat deleted successfully.");
    }
}