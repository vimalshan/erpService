namespace Shared.Core.CQRS;

/// <summary>
/// Base class for all commands in CQRS pattern
/// Commands represent an action that should be performed
/// </summary>
public abstract record Command
{
    public Guid CommandId { get; init; } = Guid.NewGuid();
    public DateTime IssuedAt { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// Base class for commands that return a result
/// </summary>
public abstract record Command<TResponse> : Command;

/// <summary>
/// Handler interface for commands
/// </summary>
public interface ICommandHandler<in TCommand> where TCommand : Command
{
    Task ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Handler interface for commands that return a response
/// </summary>
public interface ICommandHandler<in TCommand, TResponse> where TCommand : Command<TResponse>
{
    Task<TResponse> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
}
