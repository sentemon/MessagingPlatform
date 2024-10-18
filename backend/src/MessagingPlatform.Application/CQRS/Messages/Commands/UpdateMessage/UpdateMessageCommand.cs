using MediatR;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.UpdateMessage;

public record UpdateMessageCommand(UpdateMessageDto UpdateMessage, Guid UserId) : IRequest<Message>;