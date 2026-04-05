using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Entities;
using TimeSheetService.Domain.Interfaces;
using TimeSheetService.Domain.ValueObjects;

namespace TimeSheetService.Application.Commands.SubmitTimesheet;

public class SubmitTimesheetCommandHandler : IRequestHandler<SubmitTimesheetCommand, TimesheetEntryDto>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;

    public SubmitTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TimesheetEntryDto> Handle(SubmitTimesheetCommand request, CancellationToken cancellationToken)
    {
        var entryType = EntryType.FromCode(request.EntryTypeCode[0]);

        var entry = new TimesheetEntry(
            request.TimeId,
            request.EmployeeSysId,
            request.TimeDate,
            request.TimeIn,
            request.TimeOut,
            request.TotalHours,
            request.Remarks,
            entryType,
            request.ModifiedBy);

        foreach (var d in request.Details)
            entry.AddDetail(d.DetailId, d.Hours, d.ProjectId, d.SubCategoryId, d.Remarks, d.CallNo, request.ModifiedBy);

        await _repository.AddAsync(entry, cancellationToken);
        return _mapper.Map<TimesheetEntryDto>(entry);
    }
}
