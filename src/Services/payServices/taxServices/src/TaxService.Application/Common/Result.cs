namespace TaxService.Application.Common;

/// <summary>
/// Result wrapper for success/failure responses
/// </summary>
public class Result<T>
{
    private Result(bool isSuccess, T? data, string? error)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
        Errors = new List<string>();
    }

    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Error { get; }
    public List<string> Errors { get; set; }

    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(string error) => new(false, default, error);
    public static Result<T> Failure(List<string> errors) => new(false, default, null) 
    { 
        Errors = errors 
    };
}

/// <summary>
/// Base result for operations without return data
/// </summary>
public class Result
{
    private Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = new List<string>();
    }

    public bool IsSuccess { get; }
    public string? Error { get; }
    public List<string> Errors { get; set; }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
    public static Result Failure(List<string> errors) => new(false, null) 
    { 
        Errors = errors 
    };
}
