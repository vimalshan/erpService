using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Aggregates;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.Readings.Commands.InsertReading;

public class InsertReadingCommandHandler : IRequestHandler<InsertReadingCommand, EcReadingDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public InsertReadingCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EcReadingDto> Handle(InsertReadingCommand request, CancellationToken ct)
    {
        var process = await _uow.Processes.GetByIdAsync(request.ProcessId, ct)
            ?? throw new KeyNotFoundException($"Process {request.ProcessId} not found.");

        var aggregate = new EnergyProcessAggregate(process);
        var previousReading = await _uow.Readings.GetLastReadingValueAsync(request.UnitCode, request.ProcessId, ct);

        var reading = aggregate.RecordReading(
            request.UnitCode, request.ReadingValue, request.TargetValue,
            request.Remarks, request.ModifiedBy, previousReading);

        await _uow.Readings.AddAsync(reading, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<EcReadingDto>(reading);
    }
}
