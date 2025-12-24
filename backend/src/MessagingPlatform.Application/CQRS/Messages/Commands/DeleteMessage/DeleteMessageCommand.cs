using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common.Models.MessageDTOs;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.DeleteMessage;

public record DeleteMessageCommand(DeleteMessageDto DeleteMessage) : ICommand;