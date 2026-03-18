namespace FeedbackService.Application.Commands.Handlers;

using MediatR;
using DTOs;
using AutoMapper;

/// <summary>
/// Handler for SubmitFeedbackCommand
/// </summary>
public class SubmitFeedbackCommandHandler : IRequestHandler<SubmitFeedbackCommand, FeedbackDto>
{
    private readonly IFeedbackRepository _repository;
    private readonly IMapper _mapper;
    private readonly IDomainEventPublisher _eventPublisher;

    /// <summary>
    /// Initializes a new instance of the SubmitFeedbackCommandHandler class
    /// </summary>
    public SubmitFeedbackCommandHandler(
        IFeedbackRepository repository,
        IMapper mapper,
        IDomainEventPublisher eventPublisher)
    {
        _repository = repository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Handles the SubmitFeedbackCommand
    /// </summary>
    public async Task<FeedbackDto> Handle(SubmitFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _repository.GetByIdAsync(request.FeedbackId, cancellationToken);
        if (feedback == null)
            throw new KeyNotFoundException($"Feedback with ID {request.FeedbackId} not found");

        feedback.Submit();

        await _repository.UpdateAsync(feedback, cancellationToken);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        // Publish domain events
        await _eventPublisher.PublishAsync(feedback.DomainEvents, cancellationToken);

        return _mapper.Map<FeedbackDto>(feedback);
    }
}

/// <summary>
/// Interface for publishing domain events
/// </summary>
public interface IDomainEventPublisher
{
    /// <summary>
    /// Publishes domain events
    /// </summary>
    Task PublishAsync(IReadOnlyList<Domain.Common.DomainEvent> events, CancellationToken cancellationToken = default);
}
