using MediatR;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;

public record AddMessageCommand(AddMessageDto AddMessage, Guid SenderId) : IRequest<Message>;