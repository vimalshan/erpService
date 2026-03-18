using AutoMapper;
using MediatR;
using ReimbursementService.Application.DTOs;
using ReimbursementService.Domain.Interfaces;

namespace ReimbursementService.Application.Features.Reimbursements.Queries.GetReimbursementsByEmployee;

public sealed record GetReimbursementsByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<ReimbursementDto>>;

public sealed class GetReimbursementsByEmployeeQueryHandler(
    IReimbursementRepository repository,
    IMapper mapper) : IRequestHandler<GetReimbursementsByEmployeeQuery, IEnumerable<ReimbursementDto>>
{
    public async Task<IEnumerable<ReimbursementDto>> Handle(GetReimbursementsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetByEmployeeAsync(request.EmpSysId, cancellationToken);
        return mapper.Map<IEnumerable<ReimbursementDto>>(entities);
    }
}
