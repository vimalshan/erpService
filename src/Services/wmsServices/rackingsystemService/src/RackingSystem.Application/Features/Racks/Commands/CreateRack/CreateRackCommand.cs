using MediatR;
using RackingSystem.Application.Features.Racks.DTOs;

namespace RackingSystem.Application.Features.Racks.Commands.CreateRack;

public record CreateRackCommand(
    int ZoneId,
    string Code,
    string? RackType,
    decimal? MaxLoadWeight
) : IRequest<RackDto>;
