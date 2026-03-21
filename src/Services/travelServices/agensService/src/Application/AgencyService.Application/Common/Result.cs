namespace AgencyService.Application.Common;

public class Result
{
    public bool Success { get; set; }
    public required string Message { get; set; }
    public List<string>? Errors { get; set; }
    
    public static Result SuccessResult(string message = "Operation successful")
    {
        return new Result { Success = true, Message = message };
    }
    
    public static Result FailureResult(string message, List<string>? errors = null)
    {
        return new Result { Success = false, Message = message, Errors = errors };
    }
}

public class Result<T>
{
    public bool Success { get; set; }
    public required string Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    
    public static Result<T> SuccessResult(T data, string message = "Operation successful")
    {
        return new Result<T> { Success = true, Message = message, Data = data };
    }
    
    public static Result<T> FailureResult(string message, List<string>? errors = null)
    {
        return new Result<T> { Success = false, Message = message, Errors = errors };
    }
}
