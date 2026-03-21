using InsuranceService.Domain.Repositories;
using MediatR;

namespace InsuranceService.Application.Commands;

public class UpdateInsuranceStatusCommandHandler : IRequestHandler<UpdateInsuranceStatusCommand, UpdateInsuranceStatusResult>
{
    private readonly ITravelInsuranceRepository _repository;

    public UpdateInsuranceStatusCommandHandler(ITravelInsuranceRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateInsuranceStatusResult> Handle(UpdateInsuranceStatusCommand request, CancellationToken cancellationToken)
    {
        var insurance = await _repository.GetByKeyAsync(request.CompanyCode, request.PlanNumber, cancellationToken);

        if (insurance is null)
            return new UpdateInsuranceStatusResult(false, "Insurance record not found.");

        insurance.UpdateStatus(request.Status, request.CertificateNumber, request.UpdatedBy);

        await _repository.UpdateAsync(insurance, cancellationToken);

        return new UpdateInsuranceStatusResult(true, "Insurance status updated successfully.");
    }
}
