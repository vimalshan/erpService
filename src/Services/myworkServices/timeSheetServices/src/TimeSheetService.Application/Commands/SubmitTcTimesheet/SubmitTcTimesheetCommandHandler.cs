using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Interfaces;
using TimeSheetService.Domain.ValueObjects;

namespace TimeSheetService.Application.Commands.SubmitTcTimesheet;

public class SubmitTcTimesheetCommandHandler : IRequestHandler<SubmitTcTimesheetCommand, TcTimesheetEntryDto>
{
    private readonly ITcTimesheetRepository _repository;
    private readonly IMapper _mapper;

    public SubmitTcTimesheetCommandHandler(ITcTimesheetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TcTimesheetEntryDto> Handle(SubmitTcTimesheetCommand request, CancellationToken cancellationToken)
    {
        var entryType = EntryType.FromCode(request.EntryTypeCode[0]);

        var entry = new Domain.Entities.TcTimesheetEntry(
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
        return _mapper.Map<TcTimesheetEntryDto>(entry);
    }
}
