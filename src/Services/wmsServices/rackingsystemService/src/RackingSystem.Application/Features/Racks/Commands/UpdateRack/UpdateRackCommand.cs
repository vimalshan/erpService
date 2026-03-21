using MediatR;
using RackingSystem.Application.Features.Racks.DTOs;

namespace RackingSystem.Application.Features.Racks.Commands.UpdateRack;

public record UpdateRackCommand(int Id, string Code, string? RackType, decimal? MaxLoadWeight) : IRequest<RackDto>;
