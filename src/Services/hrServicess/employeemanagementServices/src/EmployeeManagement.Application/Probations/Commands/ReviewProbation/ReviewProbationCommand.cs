using EmployeeManagement.Application.Probations.DTOs;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Application.Probations.Commands.ReviewProbation;

public sealed record ReviewProbationCommand(
    long ProbationId,
    char Status,     // A=Confirmed, B=Extended, C=Terminated
    string? Rating,
    long ReviewedBy
) : IRequest<ProbationDto>;

public sealed class ReviewProbationCommandValidator : AbstractValidator<ReviewProbationCommand>
{
    public ReviewProbationCommandValidator()
    {
        RuleFor(x => x.ProbationId).GreaterThan(0);
        RuleFor(x => x.Status).Must(s => new[] { 'A', 'B', 'C' }.Contains(s))
            .WithMessage("Status must be A (Confirmed), B (Extended), or C (Terminated)");
        RuleFor(x => x.ReviewedBy).GreaterThan(0);
    }
}

public sealed class ReviewProbationCommandHandler : IRequestHandler<ReviewProbationCommand, ProbationDto>
{
    private readonly IProbationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ReviewProbationCommandHandler(IProbationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProbationDto> Handle(ReviewProbationCommand request, CancellationToken cancellationToken)
    {
        var probation = await _repository.GetByIdAsync(request.ProbationId, cancellationToken)
            ?? throw new EmployeeNotFoundException($"Probation record {request.ProbationId}");

        if (probation.ProbationStatus is 'A' or 'C')
            throw new ProbationAlreadyCompletedException(probation.EmployeeId);

        probation.Review(request.Status, request.Rating, request.ReviewedBy);
        _repository.Update(probation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ProbationDto(probation.ProbationId, probation.EmployeeId, probation.GradeId,
            probation.DueDate, probation.ProbationStatus, probation.IsExtended,
            probation.Rating, probation.CreatedOn, probation.CreatedBy);
    }
}
