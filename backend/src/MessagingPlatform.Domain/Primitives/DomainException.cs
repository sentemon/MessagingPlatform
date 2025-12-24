namespace MessagingPlatform.Domain.Primitives;

public class DomainException : InvalidOperationException
{
    public DomainException(string message) : base(message)
    {
    }
}
