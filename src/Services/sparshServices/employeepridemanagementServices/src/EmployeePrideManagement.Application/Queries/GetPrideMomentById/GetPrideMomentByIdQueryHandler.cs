using EmployeePrideManagement.Application.DTOs;
using EmployeePrideManagement.Domain.Interfaces;
using MediatR;

namespace EmployeePrideManagement.Application.Queries.GetPrideMomentById;

public class GetPrideMomentByIdQueryHandler : IRequestHandler<GetPrideMomentByIdQuery, PrideMomentDto?>
{
    private readonly IDapperPrideMomentRepository _repository;

    public GetPrideMomentByIdQueryHandler(IDapperPrideMomentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PrideMomentDto?> Handle(GetPrideMomentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync<PrideMomentDto>(request.MomentPrideId);
    }
}
