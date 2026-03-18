using FinyearAPI.Application.Commands;
using FinyearAPI.Application.Queries;
using FinyearAPI.Domain.Entities;
using FinyearAPI.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FinyearAPI.Application.Handlers
{
    /// <summary>
    /// Handler for CreateFinancialYearCommand
    /// </summary>
    public class CreateFinancialYearCommandHandler : ICommandHandler<CreateFinancialYearCommand, CreateFinancialYearResponse>
    {
        private readonly IFinancialYearAggregateRepository _repository;
        private readonly ILogger<CreateFinancialYearCommandHandler> _logger;

        public CreateFinancialYearCommandHandler(
            IFinancialYearAggregateRepository repository,
            ILogger<CreateFinancialYearCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CreateFinancialYearResponse> HandleAsync(
            CreateFinancialYearCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating financial year: {Name}", command.Name);

                // Create aggregate using factory method (encapsulates business logic)
                var aggregate = FinancialYearAggregate.Create(
                    command.FinancialYearId,
                    command.Name,
                    command.StartDate,
                    command.EndDate,
                    command.UserId);

                // Persist to repository
                var created = await _repository.AddAsync(aggregate, cancellationToken);

                // In real application, publish domain events to message bus
                // await _eventPublisher.PublishAsync(created.DomainEvents);

                return new CreateFinancialYearResponse
                {
                    Id = created.Id,
                    Name = created.Name,
                    CreatedAt = created.UpdatedOn,
                    Success = true,
                    Message = "Financial year created successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating financial year: {Name}", command.Name);
                return new CreateFinancialYearResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Handler for GetAllFinancialYearsQuery
    /// </summary>
    public class GetAllFinancialYearsQueryHandler : IQueryHandler<GetAllFinancialYearsQuery, GetAllFinancialYearsResponse>
    {
        private readonly IFinancialYearAggregateRepository _repository;
        private readonly ILogger<GetAllFinancialYearsQueryHandler> _logger;

        public GetAllFinancialYearsQueryHandler(
            IFinancialYearAggregateRepository repository,
            ILogger<GetAllFinancialYearsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GetAllFinancialYearsResponse> HandleAsync(
            GetAllFinancialYearsQuery query,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching all financial years");

                var aggregates = await _repository.GetAllAsync(cancellationToken);

                var items = aggregates
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(a => new FinancialYearQueryDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        StartDate = a.Period.StartDate,
                        EndDate = a.Period.EndDate,
                        DurationInDays = a.DurationInDays,
                        Status = a.Status.ToString(),
                        IsActive = a.IsActive,
                        UpdatedOn = a.UpdatedOn
                    })
                    .ToList();

                return new GetAllFinancialYearsResponse
                {
                    Items = items,
                    TotalCount = aggregates.Count(),
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching financial years");
                throw;
            }
        }
    }

    /// <summary>
    /// Handler for GetFinancialYearByIdQuery
    /// </summary>
    public class GetFinancialYearByIdQueryHandler : IQueryHandler<GetFinancialYearByIdQuery, FinancialYearQueryDto?>
    {
        private readonly IFinancialYearAggregateRepository _repository;
        private readonly ILogger<GetFinancialYearByIdQueryHandler> _logger;

        public GetFinancialYearByIdQueryHandler(
            IFinancialYearAggregateRepository repository,
            ILogger<GetFinancialYearByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<FinancialYearQueryDto?> HandleAsync(
            GetFinancialYearByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching financial year with ID: {Id}", query.Id);

                var aggregate = await _repository.GetByIdAsync(query.Id, cancellationToken);
                if (aggregate == null)
                    return null;

                return new FinancialYearQueryDto
                {
                    Id = aggregate.Id,
                    Name = aggregate.Name,
                    StartDate = aggregate.Period.StartDate,
                    EndDate = aggregate.Period.EndDate,
                    DurationInDays = aggregate.DurationInDays,
                    Status = aggregate.Status.ToString(),
                    IsActive = aggregate.IsActive,
                    UpdatedOn = aggregate.UpdatedOn
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching financial year with ID: {Id}", query.Id);
                throw;
            }
        }
    }
}
