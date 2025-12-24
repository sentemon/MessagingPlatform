using System.Net;
using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.Common;

public class Result<TResponse> : IResult<TResponse, Error>
{

    #pragma warning disable CS8766 
    public TResponse? Response { get; }
    public Error? Error { get; }
    #pragma warning restore CS8766
    public bool IsSuccess { get; }
    public HttpStatusCode StatusCode { get; }

    private Result(TResponse response)
    {
        Response = response;
        Error = default;
        IsSuccess = true;
        StatusCode = HttpStatusCode.OK;
    }

    private Result(Error error)
    {
        Error = error;
        Response = default;
        IsSuccess = false;
        StatusCode = HttpStatusCode.BadRequest;
    }
    
    public static Result<TResponse> Success(TResponse response)
    {
        return new Result<TResponse>(response);
    }
    
    public static Result<TResponse> Failure(Error error)
    {
        return new Result<TResponse>(error);
    }
}
