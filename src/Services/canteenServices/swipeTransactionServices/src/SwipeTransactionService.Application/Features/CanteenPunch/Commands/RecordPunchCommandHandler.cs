using MediatR;
using SwipeTransactionService.Application.DTOs;
using SwipeTransactionService.Domain.Interfaces.Repositories;
using CanteenPunchEntity = SwipeTransactionService.Domain.Entities.CanteenPunch;

namespace SwipeTransactionService.Application.Features.CanteenPunch.Commands;

public sealed class RecordPunchCommandHandler : IRequestHandler<RecordPunchCommand, CanteenPunchDto>
{
    private readonly ICanteenPunchRepository _repository;

    public RecordPunchCommandHandler(ICanteenPunchRepository repository)
        => _repository = repository;

    public async Task<CanteenPunchDto> Handle(RecordPunchCommand request, CancellationToken cancellationToken)
    {
        var punchTime = request.PunchTime ?? DateTime.UtcNow;
        var punchDate = punchTime.Date;

        var existing = await _repository.GetByEmployeeAndDateAsync(request.EmployeeSysId, punchDate, cancellationToken);

        if (existing is null)
        {
            var serial = await _repository.GetNextSerialNumberAsync(cancellationToken);
            var punch = CanteenPunchEntity.CreateCheckIn(serial, request.CanteenUnit, request.EmployeeSysId, request.CanteenUnit, punchTime);
            await _repository.AddAsync(punch, cancellationToken);
            existing = punch;
        }
        else if (request.PunchType == 'O')
        {
            existing.RecordCheckOut(punchTime);
            await _repository.UpdateAsync(existing, cancellationToken);
        }

        return new CanteenPunchDto(
            existing.SerialNumber,
            existing.CompanyCode,
            existing.EmployeeSysId,
            existing.CanteenUnit,
            existing.PunchDate,
            existing.TimeIn,
            existing.TimeOut,
            existing.WorkHours);
    }
}
