namespace Gilead.Application;

public class ServiceResult
{
    protected ServiceResult(bool succeeded, string? error, int statusCode)
    {
        Succeeded = succeeded;
        Error = error;
        StatusCode = statusCode;
    }

    public bool Succeeded { get; }
    public string? Error { get; }
    public int StatusCode { get; }

    public static ServiceResult Ok() => new(true, null, 200);
    public static ServiceResult Fail(string error, int statusCode = 400) => new(false, error, statusCode);
}

public sealed class ServiceResult<T> : ServiceResult
{
    private ServiceResult(bool succeeded, T? data, string? error, int statusCode)
        : base(succeeded, error, statusCode)
    {
        Data = data;
    }

    public T? Data { get; }

    public static ServiceResult<T> Ok(T data, int statusCode = 200) => new(true, data, null, statusCode);
    public static new ServiceResult<T> Fail(string error, int statusCode = 400) => new(false, default, error, statusCode);
}
