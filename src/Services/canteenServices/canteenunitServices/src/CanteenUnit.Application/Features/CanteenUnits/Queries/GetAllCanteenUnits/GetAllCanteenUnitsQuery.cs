using MediatR;
using CanteenUnit.Application.DTOs;

namespace CanteenUnit.Application.Features.CanteenUnits.Queries.GetAllCanteenUnits;

public record GetAllCanteenUnitsQuery : IRequest<IEnumerable<CanteenUnitMasterDto>>;
