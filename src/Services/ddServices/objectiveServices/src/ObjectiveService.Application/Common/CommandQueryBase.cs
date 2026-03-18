using MediatR;

namespace ObjectiveService.Application.Common;

public abstract class CommandBase : IRequest
{
}

public abstract class CommandBase<TResponse> : IRequest<TResponse>
{
}

public abstract class QueryBase<TResponse> : IRequest<TResponse>
{
}

public class CommandResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new();

    public static CommandResult Success(string message = "Operation completed successfully")
    {
        return new CommandResult { IsSuccess = true, Message = message };
    }

    public static CommandResult Failure(string message, List<string> errors = null)
    {
        return new CommandResult { IsSuccess = false, Message = message, Errors = errors ?? new() };
    }
}

public class CommandResult<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new();

    public static CommandResult<T> Success(T data, string message = "Operation completed successfully")
    {
        return new CommandResult<T> { IsSuccess = true, Data = data, Message = message };
    }

    public static CommandResult<T> Failure(string message, List<string> errors = null)
    {
        return new CommandResult<T> { IsSuccess = false, Message = message, Errors = errors ?? new() };
    }
}
