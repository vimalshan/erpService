using MediatR;
using TaxService.Application.DTOs;
using TaxService.Application.Queries;

namespace TaxService.API.GraphQL;

public class TaxQuery
{
    public async Task<IEnumerable<ConditionalMasterDto>> GetActiveConditionalMasters(
        [Service] IMediator mediator,
        int? financialYear = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetActiveConditionalMastersQuery(financialYear), cancellationToken);
        return result.IsSuccess ? result.Data! : Enumerable.Empty<ConditionalMasterDto>();
    }

    public async Task<ConditionalMasterDto?> GetConditionalMasterById(
        long id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetConditionalMasterByIdQuery(id), cancellationToken);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<ConditionalMasterDto?> GetConditionalMasterByPayee(
        string payeeId,
        [Service] IMediator mediator,
        int? financialYear = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetConditionalMasterByPayeeIdQuery(payeeId, financialYear), cancellationToken);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<IEnumerable<TaxMarginalDetailDto>> GetEmployeeTaxDetails(
        long employeeSystemId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetEmployeeTaxDetailsQuery(employeeSystemId), cancellationToken);
        return result.IsSuccess ? result.Data! : Enumerable.Empty<TaxMarginalDetailDto>();
    }

    public async Task<TaxMarginalDetailDto?> GetTaxMarginalDetailById(
        long id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetTaxMarginalDetailByIdQuery(id), cancellationToken);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<TaxMarginalDetailDto?> GetTaxByEmployeeAndYear(
        long employeeSystemId,
        int financialYear,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetTaxByEmployeeAndYearQuery(employeeSystemId, financialYear), cancellationToken);
        return result.IsSuccess ? result.Data : null;
    }
}
