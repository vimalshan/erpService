using MediatR;

namespace CanteenUnit.Application.Features.CanteenUnits.Commands.DeleteCanteenUnit;

public record DeleteCanteenUnitCommand(decimal ComCode) : IRequest;
