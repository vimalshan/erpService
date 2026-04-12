using MediatR;
using travelTransactionService.Application.Commands;
using travelTransactionService.Application.DTOs;

namespace travelTransactionService.API.GraphQL;

public class TransactionMutation
{
    public async Task<VendorMasterDto> CreateVendor(
        CreateVendorCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<bool> UpdateVendor(
        UpdateVendorCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<bool> DeleteVendor(
        DeleteVendorCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<TaxMasterDto> CreateTaxMaster(
        CreateTaxMasterCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<bool> UpdateTaxRate(
        UpdateTaxRateCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<JaiInterfaceLineDto> CreateJaiInterfaceLine(
        CreateJaiInterfaceLineCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<bool> UpdateGstAmounts(
        UpdateGstAmountsCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<TravelApParamsDto> CreateTravelApParams(
        CreateTravelApParamsCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }
}
