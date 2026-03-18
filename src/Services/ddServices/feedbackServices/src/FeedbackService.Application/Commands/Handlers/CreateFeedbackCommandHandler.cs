namespace FeedbackService.Application.Commands.Handlers;

using MediatR;
using Domain.Aggregates;
using DTOs;
using AutoMapper;

/// <summary>
/// Handler for CreateFeedbackCommand
/// </summary>
public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, FeedbackDto>
{
    private readonly IFeedbackRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the CreateFeedbackCommandHandler class
    /// </summary>
    public CreateFeedbackCommandHandler(IFeedbackRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateFeedbackCommand
    /// </summary>
    public async Task<FeedbackDto> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = Feedback.Create(request.FeedbackId, request.RequestNo, request.ApproverSystemId);
        
        if (!string.IsNullOrEmpty(request.Remarks))
        {
            feedback.UpdateRemarks(request.Remarks);
        }

        await _repository.AddAsync(feedback, cancellationToken);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FeedbackDto>(feedback);
    }
}

/// <summary>
/// Interface for feedback repository
/// </summary>
public interface IFeedbackRepository
{
    /// <summary>
    /// Gets the unit of work
    /// </summary>
    IUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Adds a feedback to the repository
    /// </summary>
    Task AddAsync(Feedback feedback, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a feedback by ID
    /// </summary>
    Task<Feedback?> GetByIdAsync(decimal id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all feedback
    /// </summary>
    Task<List<Feedback>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets feedback by request number
    /// </summary>
    Task<List<Feedback>> GetByRequestNoAsync(decimal requestNo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a feedback
    /// </summary>
    Task UpdateAsync(Feedback feedback, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a feedback
    /// </summary>
    Task DeleteAsync(decimal id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for unit of work pattern
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all changes asynchronously
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
