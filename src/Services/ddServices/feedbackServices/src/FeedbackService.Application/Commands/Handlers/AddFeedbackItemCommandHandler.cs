namespace FeedbackService.Application.Commands.Handlers;

using MediatR;
using DTOs;
using AutoMapper;

/// <summary>
/// Handler for AddFeedbackItemCommand
/// </summary>
public class AddFeedbackItemCommandHandler : IRequestHandler<AddFeedbackItemCommand, FeedbackDto>
{
    private readonly IFeedbackRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the AddFeedbackItemCommandHandler class
    /// </summary>
    public AddFeedbackItemCommandHandler(IFeedbackRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the AddFeedbackItemCommand
    /// </summary>
    public async Task<FeedbackDto> Handle(AddFeedbackItemCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _repository.GetByIdAsync(request.FeedbackId, cancellationToken);
        if (feedback == null)
            throw new KeyNotFoundException($"Feedback with ID {request.FeedbackId} not found");

        feedback.AddItem(request.QuestionNo, request.AnswerNo);

        await _repository.UpdateAsync(feedback, cancellationToken);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FeedbackDto>(feedback);
    }
}
