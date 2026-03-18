using DeductionService.Domain.Exceptions;
using DeductionService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace DeductionService.Application.CQRS.Commands.CancelDeduction;

public record CancelDeductionCommand(
    long SystemId,
    long CancelledByUserId) : IRequest<bool>;

public class CancelDeductionCommandValidator : AbstractValidator<CancelDeductionCommand>
{
    public CancelDeductionCommandValidator()
    {
        RuleFor(x => x.SystemId).GreaterThan(0);
        RuleFor(x => x.CancelledByUserId).GreaterThan(0);
    }
}

public class CancelDeductionCommandHandler(
    IAdhocPayDeductionRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelDeductionCommand, bool>
{
    public async Task<bool> Handle(CancelDeductionCommand request, CancellationToken ct)
    {
        var deduction = await repository.GetByIdAsync(request.SystemId, ct)
            ?? throw new DeductionDomainException($"Deduction {request.SystemId} not found.");

        deduction.Cancel(request.CancelledByUserId);
        await repository.UpdateAsync(deduction, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
