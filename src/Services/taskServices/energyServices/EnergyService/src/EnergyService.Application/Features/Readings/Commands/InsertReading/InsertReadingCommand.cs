using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.Readings.Commands.InsertReading;

public record InsertReadingCommand(
    string UnitCode,
    int ProcessId,
    long ReadingValue,
    long? TargetValue,
    string? Remarks,
    int ModifiedBy) : IRequest<EcReadingDto>;
