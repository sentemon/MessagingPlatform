using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common.Models.MessageDTOs;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.UpdateMessage;

public record UpdateMessageCommand(UpdateMessageDto UpdateMessage, Guid UserId) : ICommand;