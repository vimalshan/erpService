using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Queries.GetTimeInfo;

public sealed class GetTimeInfoByEmployeeQueryHandler
    : IRequestHandler<GetTimeInfoByEmployeeQuery, IEnumerable<EmployeeTimeInfoDto>>
{
    private readonly IEmployeeTimeInfoRepository _repo;
    public GetTimeInfoByEmployeeQueryHandler(IEmployeeTimeInfoRepository repo) => _repo = repo;

    public async Task<IEnumerable<EmployeeTimeInfoDto>> Handle(GetTimeInfoByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByEmployeeIdAsync(request.EmpSysId, cancellationToken);
        return items.Select(i => new EmployeeTimeInfoDto(
            i.TimeInfoId, i.EmpSysId.Value, i.EmpAttFlag.Value, i.LastModifiedBy, i.LastModifiedOn));
    }
}

public sealed class GetTimeInfoByIdQueryHandler
    : IRequestHandler<GetTimeInfoByIdQuery, EmployeeTimeInfoDto?>
{
    private readonly IEmployeeTimeInfoRepository _repo;
    public GetTimeInfoByIdQueryHandler(IEmployeeTimeInfoRepository repo) => _repo = repo;

    public async Task<EmployeeTimeInfoDto?> Handle(GetTimeInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var i = await _repo.GetByIdAsync(request.TimeInfoId, cancellationToken);
        return i is null ? null : new EmployeeTimeInfoDto(
            i.TimeInfoId, i.EmpSysId.Value, i.EmpAttFlag.Value, i.LastModifiedBy, i.LastModifiedOn);
    }
}
