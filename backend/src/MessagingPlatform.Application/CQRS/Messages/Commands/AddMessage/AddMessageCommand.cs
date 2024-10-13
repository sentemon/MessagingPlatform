using MediatR;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;

public record AddMessageCommand(CreateMessageDto CreateMessage, Guid SenderId) : IRequest<Message>;