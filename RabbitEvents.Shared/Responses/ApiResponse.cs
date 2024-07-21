using System.Net;

namespace RabbitEvents.Shared.Responses;

public class ApiResponse<T>
{
    public HttpStatusCode StatusCode { get; init; }
    public bool Success { get; init; }
    public T? Data { get; init; }
    public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();

    public ApiResponse()
    {

    }

    public ApiResponse(HttpStatusCode statusCode, bool success)
    {
        StatusCode = statusCode;
        Success = success;
    }

    public ApiResponse(HttpStatusCode statusCode, bool sucesso, T data) : this(statusCode, sucesso) => Data = data;

    public ApiResponse(HttpStatusCode statusCode, bool sucesso, IEnumerable<string> erros) : this(statusCode, sucesso) =>
        Errors = erros;

    public ApiResponse(HttpStatusCode statusCode, bool sucesso, T data, IEnumerable<string> erros) : this(statusCode, sucesso, data) =>
        Errors = erros;

    public ApiResponse<T> OkResponse(T data) => new ApiResponse<T>(HttpStatusCode.OK, true, data);

    public ApiResponse<T> CreatedResponse(T data) => new ApiResponse<T>(HttpStatusCode.Created, true, data);

    public ApiResponse<T> NotFoundResponse() => new ApiResponse<T>(HttpStatusCode.NotFound, true);

    public ApiResponse<T> BadRequestResponse(IEnumerable<string> erros) => new ApiResponse<T>(HttpStatusCode.BadRequest, false, erros);
}
