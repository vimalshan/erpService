using MediatR;

namespace LocationServices.Application.Abstractions;

/// <summary>CQRS — Command marker (changes state)</summary>
public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

/// <summary>CQRS — Query marker (reads state)</summary>
public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

/// <summary>Command handler base</summary>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand { }

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse> { }

/// <summary>Query handler base</summary>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse> { }

/// <summary>Result pattern — avoids exceptions for expected failures</summary>
public sealed class Result
{
    public bool IsSuccess { get; }
    public string? Error  { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, string? error) { IsSuccess = isSuccess; Error = error; }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}

public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public string? Error  { get; }
    public T? Value       { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, T? value, string? error)
    { IsSuccess = isSuccess; Value = value; Error = error; }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}
