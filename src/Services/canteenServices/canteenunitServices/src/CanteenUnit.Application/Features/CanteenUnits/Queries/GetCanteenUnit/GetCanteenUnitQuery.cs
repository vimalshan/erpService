using MediatR;
using CanteenUnit.Application.DTOs;

namespace CanteenUnit.Application.Features.CanteenUnits.Queries.GetCanteenUnit;

public record GetCanteenUnitQuery(decimal ComCode) : IRequest<CanteenUnitMasterDto?>;
