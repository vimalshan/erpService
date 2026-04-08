using FluentValidation;
using MediatR;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.UpdateAdvanceLicense;

public sealed record UpdateAdvanceLicenseCommand(
    long LicenseId, string? LicenseNo, int? FgCode,
    decimal? ExportObligationAmount, decimal? ExportAmount) : IRequest;

public sealed class UpdateAdvanceLicenseCommandValidator : AbstractValidator<UpdateAdvanceLicenseCommand>
{
    public UpdateAdvanceLicenseCommandValidator()
    {
        RuleFor(x => x.LicenseId).GreaterThan(0);
        RuleFor(x => x.LicenseNo).MaximumLength(40).When(x => x.LicenseNo is not null);
    }
}

public sealed class UpdateAdvanceLicenseCommandHandler(IAdvanceLicenseRepository repository)
    : IRequestHandler<UpdateAdvanceLicenseCommand>
{
    public async Task Handle(UpdateAdvanceLicenseCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.LicenseId, cancellationToken)
            ?? throw new KeyNotFoundException($"Advance license {request.LicenseId} not found.");

        entity.Update(request.LicenseNo, request.FgCode,
            request.ExportObligationAmount, request.ExportAmount);
        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
