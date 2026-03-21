using InsuranceService.Application.Commands;
using InsuranceService.Application.DTOs;
using InsuranceService.Application.Queries;
using MediatR;

namespace InsuranceService.API.GraphQL;

public class InsuranceQuery
{
    public async Task<IReadOnlyList<TravelInsuranceDto>> GetInsurances(
        [Service] IMediator mediator,
        string? companyCode = null,
        long? planNumber = null,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetInsuranceDetailsQuery(companyCode, planNumber), cancellationToken);
    }

    public async Task<TravelInsuranceDto?> GetInsuranceByKey(
        [Service] IMediator mediator,
        string companyCode,
        long planNumber,
        CancellationToken cancellationToken = default)
    {
        var results = await mediator.Send(
            new GetInsuranceDetailsQuery(companyCode, planNumber), cancellationToken);
        return results.FirstOrDefault();
    }
}

public class InsuranceMutation
{
    public async Task<RegisterInsuranceResult> RegisterInsurance(
        [Service] IMediator mediator,
        string companyCode,
        long planNumber,
        string insuranceType,
        string? passportNumber,
        string? visaPlace,
        string? nominee1,
        string? nominee2,
        string? remarks,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new RegisterInsuranceCommand(
            companyCode, planNumber, insuranceType,
            passportNumber, visaPlace, nominee1, nominee2, remarks), cancellationToken);
    }

    public async Task<UpdateInsuranceStatusResult> UpdateInsuranceStatus(
        [Service] IMediator mediator,
        string companyCode,
        long planNumber,
        string status,
        string? certificateNumber,
        long? updatedBy,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new UpdateInsuranceStatusCommand(
            companyCode, planNumber, status, certificateNumber, updatedBy), cancellationToken);
    }
}
