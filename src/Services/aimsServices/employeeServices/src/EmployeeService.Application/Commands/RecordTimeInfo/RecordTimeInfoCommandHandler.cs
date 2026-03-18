using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Commands.RecordTimeInfo;

public sealed class RecordTimeInfoCommandHandler : IRequestHandler<RecordTimeInfoCommand, EmployeeTimeInfoDto>
{
    private readonly IEmployeeTimeInfoRepository _repo;

    public RecordTimeInfoCommandHandler(IEmployeeTimeInfoRepository repo) => _repo = repo;

    public async Task<EmployeeTimeInfoDto> Handle(RecordTimeInfoCommand request, CancellationToken cancellationToken)
    {
        var nextId = await _repo.GetNextIdAsync(cancellationToken);
        var info = EmployeeTimeInfo.Create(nextId, request.EmpSysId, request.AttFlag, request.ModifiedBy);
        await _repo.AddAsync(info, cancellationToken);

        return new EmployeeTimeInfoDto(
            info.TimeInfoId,
            info.EmpSysId.Value,
            info.EmpAttFlag.Value,
            info.LastModifiedBy,
            info.LastModifiedOn
        );
    }
}
