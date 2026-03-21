using InsuranceService.Domain.Entities;
using InsuranceService.Domain.Repositories;
using MediatR;

namespace InsuranceService.Application.Commands;

public class RegisterInsuranceCommandHandler : IRequestHandler<RegisterInsuranceCommand, RegisterInsuranceResult>
{
    private readonly ITravelInsuranceRepository _repository;

    public RegisterInsuranceCommandHandler(ITravelInsuranceRepository repository)
    {
        _repository = repository;
    }

    public async Task<RegisterInsuranceResult> Handle(RegisterInsuranceCommand request, CancellationToken cancellationToken)
    {
        var insurance = TravelInsurance.Register(
            request.CompanyCode,
            request.PlanNumber,
            request.InsuranceType,
            request.PassportNumber,
            request.VisaPlace,
            request.Nominee1,
            request.Nominee2,
            request.Remarks);

        await _repository.AddAsync(insurance, cancellationToken);

        return new RegisterInsuranceResult(true, "Travel insurance registered successfully", request.PlanNumber);
    }
}
