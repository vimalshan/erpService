using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Interfaces;

namespace TimeSheetService.Application.Commands.UpdateTimesheet;

public class UpdateTimesheetCommandHandler : IRequestHandler<UpdateTimesheetCommand, TimesheetEntryDto>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;

    public UpdateTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TimesheetEntryDto> Handle(UpdateTimesheetCommand request, CancellationToken cancellationToken)
    {
        var entry = await _repository.GetByIdAsync(request.TimeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Timesheet {request.TimeId} not found.");

        entry.Update(request.TimeIn, request.TimeOut, request.TotalHours, request.Remarks, request.ModifiedBy);
        await _repository.UpdateAsync(entry, cancellationToken);
        return _mapper.Map<TimesheetEntryDto>(entry);
    }
}
