using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common.Models.MessageDTOs;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;

public record AddMessageCommand(CreateMessageDto CreateMessage, Guid SenderId) : ICommand;