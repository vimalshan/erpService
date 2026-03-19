using HotChocolate.Language;
using HotChocolate.Types;
using MediatR;
using VehicleTracking.Application.DTOs;
using VehicleTracking.Application.Vehicles.Commands;
using VehicleTracking.Application.Vehicles.Queries;

namespace VehicleTracking.API.GraphQL;

public class CharType : ScalarType<char, StringValueNode>
{
    public CharType() : base("Char") { }

    protected override char ParseLiteral(StringValueNode valueSyntax)
        => valueSyntax.Value.Length > 0 ? valueSyntax.Value[0] : default;

    protected override StringValueNode ParseValue(char runtimeValue)
        => new(runtimeValue.ToString());

    public override IValueNode ParseResult(object? resultValue)
        => resultValue is char c ? ParseValue(c) : NullValueNode.Default;
}

public class VehicleQuery
{
    public async Task<IEnumerable<VehicleMasterDto>> GetVehicles([Service] IMediator mediator)
        => await mediator.Send(new GetAllVehiclesQuery());

    public async Task<VehicleMasterDto?> GetVehicleById([Service] IMediator mediator, long serialNumber)
        => await mediator.Send(new GetVehicleByIdQuery(serialNumber));

    public async Task<IEnumerable<VehicleStageDto>> GetVehicleStages([Service] IMediator mediator, long trackingNumber)
        => await mediator.Send(new GetVehicleStagesQuery(trackingNumber));

    public async Task<IEnumerable<VehicleTransactionDto>> GetActiveTransactions([Service] IMediator mediator)
        => await mediator.Send(new GetActiveTransactionsQuery());

    public async Task<IEnumerable<StageMasterDto>> GetStages([Service] IMediator mediator)
        => await mediator.Send(new GetAllStagesQuery());

    public async Task<IEnumerable<PurposeMasterDto>> GetPurposes([Service] IMediator mediator)
        => await mediator.Send(new GetAllPurposesQuery());

    public async Task<PurposeMasterDto?> GetPurposeWithStages([Service] IMediator mediator, long purposeCode)
        => await mediator.Send(new GetPurposeWithStagesQuery(purposeCode));

    public async Task<WeightInfoDto?> GetWeightInfo([Service] IMediator mediator, long trackingNumber)
        => await mediator.Send(new GetWeightInfoQuery(trackingNumber));
}

public class VehicleMutation
{
    public async Task<VehicleMasterDto> RegisterVehicle([Service] IMediator mediator, RegisterVehicleCommand input)
        => await mediator.Send(input);

    public async Task<VehicleStageDto> UpdateVehicleStage([Service] IMediator mediator, UpdateVehicleStageCommand input)
        => await mediator.Send(input);

    public async Task<VehicleTransactionDto> CreateTransaction([Service] IMediator mediator, CreateVehicleTransactionCommand input)
        => await mediator.Send(input);

    public async Task<VehicleInvoiceDto> CreateInvoice([Service] IMediator mediator, CreateVehicleInvoiceCommand input)
        => await mediator.Send(input);

    public async Task<DecisionFlagDto> MakeDecision([Service] IMediator mediator, MakeDecisionCommand input)
        => await mediator.Send(input);

    public async Task<VehicleMasterDto> UpdateVehicle([Service] IMediator mediator, UpdateVehicleMasterCommand input)
        => await mediator.Send(input);

    public async Task<VehicleTransactionDto> CloseTransaction([Service] IMediator mediator, CloseVehicleTransactionCommand input)
        => await mediator.Send(input);

    public async Task<WeightInfoDto> UpdateWeightInfo([Service] IMediator mediator, UpdateWeightInfoCommand input)
        => await mediator.Send(input);
}
