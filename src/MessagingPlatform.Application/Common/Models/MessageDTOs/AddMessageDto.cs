namespace MessagingPlatform.Application.Common.Models.MessageDTOs
{
    public class AddMessageDto
    {
        public required Guid SenderId { get; set; }
        public required Guid ChatId { get; set; }
        public required string Content { get; set; }
    }
}