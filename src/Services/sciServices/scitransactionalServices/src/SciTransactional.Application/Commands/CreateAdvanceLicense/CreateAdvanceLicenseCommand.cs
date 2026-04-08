using FluentValidation;
using MediatR;
using SciTransactional.Domain.Entities;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.CreateAdvanceLicense;

public sealed record CreateAdvanceLicenseCommand(
    long LicenseId, string? LicenseNo, int? FgCode,
    decimal? ExportObligationAmount, decimal? ExportAmount,
    List<int>? EntitlementRms) : IRequest<long>;

public sealed class CreateAdvanceLicenseCommandValidator : AbstractValidator<CreateAdvanceLicenseCommand>
{
    public CreateAdvanceLicenseCommandValidator()
    {
        RuleFor(x => x.LicenseId).GreaterThan(0);
        RuleFor(x => x.LicenseNo).MaximumLength(40).When(x => x.LicenseNo is not null);
    }
}

public sealed class CreateAdvanceLicenseCommandHandler(IAdvanceLicenseRepository repository)
    : IRequestHandler<CreateAdvanceLicenseCommand, long>
{
    public async Task<long> Handle(CreateAdvanceLicenseCommand request, CancellationToken cancellationToken)
    {
        var entity = AdvanceLicenseEntity.Create(
            request.LicenseId, request.LicenseNo, request.FgCode,
            request.ExportObligationAmount, request.ExportAmount);

        if (request.EntitlementRms is { Count: > 0 })
        {
            foreach (var rm in request.EntitlementRms)
            {
                entity.AddEntitlement(
                    AdvanceLicenseEntitlementEntity.Create(request.LicenseId, rm));
            }
        }

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
