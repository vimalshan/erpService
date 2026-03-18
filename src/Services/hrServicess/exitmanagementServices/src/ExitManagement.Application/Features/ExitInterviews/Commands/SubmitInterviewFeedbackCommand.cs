using ExitManagement.Domain.Entities;
using ExitManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace ExitManagement.Application.Features.ExitInterviews.Commands;

public record SubmitInterviewFeedbackCommand(
    decimal ExitNo,
    decimal SerialNo,
    string QuestionId,
    string Feedback,
    decimal UpdatedBy
) : IRequest<Unit>;

public class SubmitInterviewFeedbackValidator : AbstractValidator<SubmitInterviewFeedbackCommand>
{
    public SubmitInterviewFeedbackValidator()
    {
        RuleFor(x => x.ExitNo).GreaterThan(0);
        RuleFor(x => x.QuestionId).NotEmpty().MaximumLength(4);
        RuleFor(x => x.Feedback).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class SubmitInterviewFeedbackHandler : IRequestHandler<SubmitInterviewFeedbackCommand, Unit>
{
    private readonly IExitInterviewFeedbackRepository _repository;

    public SubmitInterviewFeedbackHandler(IExitInterviewFeedbackRepository repository)
        => _repository = repository;

    public async Task<Unit> Handle(SubmitInterviewFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = ExitInterviewFeedback.Create(
            request.ExitNo, request.SerialNo, request.QuestionId,
            request.Feedback, request.UpdatedBy);

        await _repository.AddAsync(feedback, cancellationToken);
        return Unit.Value;
    }
}
