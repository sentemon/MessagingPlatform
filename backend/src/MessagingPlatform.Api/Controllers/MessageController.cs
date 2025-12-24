using System.Security.Claims;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;
using MessagingPlatform.Application.CQRS.Messages.Commands.DeleteMessage;
using MessagingPlatform.Application.CQRS.Messages.Commands.UpdateMessage;
using MessagingPlatform.Application.CQRS.Messages.Queries.GetAllMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Controllers;

[Authorize]
[Route("api/chats/{chatId:guid}/messages")]
[ApiController]
public class MessageController : ControllerBase
{

    private readonly GetAllMessagesQueryHandler _getAllMessagesQueryHandler;
    private readonly AddMessageCommandHandler _addMessageCommandHandler;
    private readonly UpdateMessageCommandHandler _updateMessageCommandHandler;
    private readonly DeleteMessageCommandHandler _deleteMessageCommandHandler;
    
    public MessageController(GetAllMessagesQueryHandler getAllMessagesQueryHandler, AddMessageCommandHandler addMessageCommandHandler, UpdateMessageCommandHandler updateMessageCommandHandler, DeleteMessageCommandHandler deleteMessageCommandHandler)
    {
        _getAllMessagesQueryHandler = getAllMessagesQueryHandler;
        _addMessageCommandHandler = addMessageCommandHandler;
        _updateMessageCommandHandler = updateMessageCommandHandler;
        _deleteMessageCommandHandler = deleteMessageCommandHandler;
    }
    
    // ToDo: use instead of navigation property "Messages" in Chat entity for better productivity
    [HttpGet]
    public async Task<IActionResult> GetAll(Guid chatId)
    {
        var query = new GetAllMessagesQuery(chatId);
        var result = await _getAllMessagesQueryHandler.Handle(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }

        return Ok(result.Response);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Guid chatId, [FromBody] CreateMessageDto createMessage)
    {
        var senderId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);

        createMessage.ChatId = chatId;

        var command = new AddMessageCommand(createMessage, senderId);
        var result =  await _addMessageCommandHandler.Handle(command);

        // var messageDto = _mapper.Map<GetMessageDto>(message);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Response);
    }

    [HttpPut("{messageId:guid}")]
    public async Task<IActionResult> Update(Guid chatId, Guid messageId, [FromBody] UpdateMessageDto updateMessage)
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);

        updateMessage.MessageId = messageId;
        var command = new UpdateMessageCommand(updateMessage, userId);
        var result = await _updateMessageCommandHandler.Handle(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Response);
    }

    [HttpDelete("{messageId:guid}")]
    public async Task<IActionResult> Delete(Guid chatId, Guid messageId)
    {
        var senderId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        var command = new DeleteMessageCommand(new DeleteMessageDto { MessageId = messageId, SenderId = senderId });
        var result = await _deleteMessageCommandHandler.Handle(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error.Message);
        }

        return Ok(result.Response);
    }
}