using System.Security.Claims;
using AutoMapper;
using MediatR;
using MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;
using MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;
using MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using MessagingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ChatController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
        
    [HttpGet("getall")]
    public async Task<IActionResult> GetChats()
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        var chats = await _mediator.Send(new GetChatsQuery(userId));

        return Ok(chats);
    }
        
    [HttpGet("get")]
    public async Task<IActionResult> GetChat(Guid id)
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        var chat = await _mediator.Send(new GetChatByIdQuery(id, userId));

        if (chat == null)
        {
            return NotFound();
        }

        var chatDto = _mapper.Map<GetChatDto>(chat);
        
        return Ok(chatDto);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChatDto)
    {
        var chatRequest = _mapper.Map<Chat>(createChatDto);
        
        var usernames = createChatDto.Usernames;

        var creatorId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        var chatResponse = await _mediator.Send(new CreateChatCommand(chatRequest, usernames, creatorId));

        return Ok(chatResponse.Id); // maybe return the entity of chat
    }
        
    [HttpPut("update")]
    public async Task<IActionResult> UpdateChat([FromBody] UpdateChatDto updateChatDto)
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        var updatedChat = await _mediator.Send(new UpdateChatCommand(updateChatDto, userId));

        if (!updatedChat)
        {
            NotFound("Chat not found");
        }
    
        return Ok("Chat updated successfully.");
    }
        
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteChat(Guid id)
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        var result = await _mediator.Send(new DeleteChatCommand(id, userId));

        if (!result)
        {
            // ToDo: another response
            return NotFound("Chat not found or you do not have permissions to delete it.");
        }

        return Ok("Chat deleted successfully.");
    }
}