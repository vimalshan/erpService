using MediatR;

namespace CanteenUnit.Application.Features.CanteenUnits.Commands.UpdateCanteenUnit;

public record UpdateCanteenUnitCommand(
    decimal ComCode,
    string UnitName,
    string? UnitRef,
    decimal? MaxVal,
    decimal? MinVal,
    long? SiteId,
    long? HrmsId) : IRequest;
