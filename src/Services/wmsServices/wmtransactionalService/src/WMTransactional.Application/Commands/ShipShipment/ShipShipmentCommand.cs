using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.ShipShipment;

public record ShipShipmentCommand(int ShipmentId) : IRequest<ShipmentDto>;
