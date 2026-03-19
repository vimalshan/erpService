using EmployeePrideManagement.Application.DTOs;
using EmployeePrideManagement.Domain.Interfaces;
using MediatR;

namespace EmployeePrideManagement.Application.Queries.GetPrideMomentsByEmployee;

public class GetPrideMomentsByEmployeeQueryHandler : IRequestHandler<GetPrideMomentsByEmployeeQuery, IEnumerable<PrideMomentDto>>
{
    private readonly IDapperPrideMomentRepository _repository;

    public GetPrideMomentsByEmployeeQueryHandler(IDapperPrideMomentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PrideMomentDto>> Handle(GetPrideMomentsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByEmployeeIdAsync<PrideMomentDto>(request.EmployeeSysId);
    }
}
