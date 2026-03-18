namespace FinyearAPI.Application.Commands
{
    /// <summary>
    /// Base interface for all commands in CQRS pattern
    /// </summary>
    public interface ICommand<TResponse>
    {
    }

    /// <summary>
    /// Base interface for command handlers
    /// </summary>
    public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        /// <summary>
        /// Handle the command
        /// </summary>
        Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Command to create a new financial year
    /// </summary>
    public class CreateFinancialYearCommand : ICommand<CreateFinancialYearResponse>
    {
        public long FinancialYearId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long UserId { get; set; }
    }

    /// <summary>
    /// Response for create command
    /// </summary>
    public class CreateFinancialYearResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Command to update a financial year
    /// </summary>
    public class UpdateFinancialYearCommand : ICommand<UpdateFinancialYearResponse>
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long UserId { get; set; }
    }

    /// <summary>
    /// Response for update command
    /// </summary>
    public class UpdateFinancialYearResponse
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Command to close a financial year
    /// </summary>
    public class CloseFinancialYearCommand : ICommand<CloseFinancialYearResponse>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
    }

    /// <summary>
    /// Response for close command
    /// </summary>
    public class CloseFinancialYearResponse
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Command to delete a financial year
    /// </summary>
    public class DeleteFinancialYearCommand : ICommand<DeleteFinancialYearResponse>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
    }

    /// <summary>
    /// Response for delete command
    /// </summary>
    public class DeleteFinancialYearResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
