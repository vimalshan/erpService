using MediatR;
using travelTransactionService.Application.DTOs;
using travelTransactionService.Application.Queries;

namespace travelTransactionService.API.GraphQL;

public class TransactionQuery
{
    public async Task<IReadOnlyList<VendorMasterDto>> GetVendors(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllVendorsQuery(), cancellationToken);
    }

    public async Task<VendorMasterDto?> GetVendorById(
        long vendorId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetVendorByIdQuery(vendorId), cancellationToken);
    }

    public async Task<IReadOnlyList<TaxMasterDto>> GetTaxMasters(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllTaxMastersQuery(), cancellationToken);
    }

    public async Task<TaxMasterDto?> GetTaxMasterByType(
        string taxType,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetTaxMasterByTypeQuery(taxType), cancellationToken);
    }

    public async Task<IReadOnlyList<JaiInterfaceLineDto>> GetJaiInterfaceLines(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllJaiInterfaceLinesQuery(), cancellationToken);
    }

    public async Task<IReadOnlyList<AccountMasterDto>> GetAccountMasters(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllAccountMastersQuery(), cancellationToken);
    }

    public async Task<IReadOnlyList<GlCodeCombinationDto>> GetGlCodeCombinations(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllGlCodeCombinationsQuery(), cancellationToken);
    }

    public async Task<IReadOnlyList<JvInterfaceDto>> GetJvInterfaces(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllJvInterfacesQuery(), cancellationToken);
    }

    public async Task<IReadOnlyList<TravelApParamsDto>> GetTravelApParams(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllTravelApParamsQuery(), cancellationToken);
    }

    public async Task<IReadOnlyList<SourceHistoryDto>> GetSourceHistory(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllSourceHistoryQuery(), cancellationToken);
    }
}
