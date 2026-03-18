using MediatR;
using LeaveServices.Application.DTOs;
using LeaveServices.Domain.Repositories;

namespace LeaveServices.Application.Features.LossOfPay.Queries;

public sealed class GetLossOfPayByEmployeeHandler : IRequestHandler<GetLossOfPayByEmployeeQuery, IEnumerable<LossOfPayDto>>
{
    private readonly ILossOfPayRepository _repository;
    public GetLossOfPayByEmployeeHandler(ILossOfPayRepository repository) => _repository = repository;

    public async Task<IEnumerable<LossOfPayDto>> Handle(GetLossOfPayByEmployeeQuery request, CancellationToken ct)
    {
        var list = await _repository.GetByEmployeeAsync(request.EmpSysId, ct);
        return list.Select(e => new LossOfPayDto(e.LopId, e.EmpSysId, e.LopDays,
            e.LopMonth, e.LopRemarks, e.CreatedOn));
    }
}
