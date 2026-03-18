using MediatR;
using CanteenUnit.Application.DTOs;

namespace CanteenUnit.Application.Features.CanteenUnits.Commands.CreateCanteenUnit;

public record CreateCanteenUnitCommand(
    decimal ComCode,
    string UnitName,
    string? UnitRef,
    decimal? MaxVal,
    decimal? MinVal,
    long? SiteId,
    long? HrmsId) : IRequest<CanteenUnitMasterDto>;
