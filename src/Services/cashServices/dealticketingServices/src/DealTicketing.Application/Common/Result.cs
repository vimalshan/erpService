namespace DealTicketing.Application.Common;

/// <summary>Wrapper for application operation results, replacing thrown exceptions for expected failures.</summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(T value) { IsSuccess = true; Value = value; }
    private Result(string error) { IsSuccess = false; Error = error; }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);
}

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }

    private Result() { IsSuccess = true; }
    private Result(string error) { IsSuccess = false; Error = error; }

    public static Result Success() => new();
    public static Result Failure(string error) => new(error);
}

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
