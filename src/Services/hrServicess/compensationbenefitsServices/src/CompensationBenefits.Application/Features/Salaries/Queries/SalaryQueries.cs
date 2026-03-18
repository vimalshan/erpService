using AutoMapper;
using CompensationBenefits.Application.DTOs;
using CompensationBenefits.Domain.Interfaces;
using MediatR;

namespace CompensationBenefits.Application.Features.Salaries.Queries;

// ─── GetById ───────────────────────────────────────────────────────────────────
public record GetSalaryByIdQuery(long SalaryId) : IRequest<SalaryDto?>;

public class GetSalaryByIdQueryHandler(ISalaryRepository repo, IMapper mapper)
    : IRequestHandler<GetSalaryByIdQuery, SalaryDto?>
{
    public async Task<SalaryDto?> Handle(GetSalaryByIdQuery request, CancellationToken ct)
    {
        var salary = await repo.GetWithDetailsAsync(request.SalaryId, ct);
        return salary is null ? null : mapper.Map<SalaryDto>(salary);
    }
}

// ─── GetAll ─────────────────────────────────────────────────────────────────────
public record GetAllSalariesQuery : IRequest<IEnumerable<SalaryDto>>;

public class GetAllSalariesQueryHandler(ISalaryRepository repo, IMapper mapper)
    : IRequestHandler<GetAllSalariesQuery, IEnumerable<SalaryDto>>
{
    public async Task<IEnumerable<SalaryDto>> Handle(GetAllSalariesQuery request, CancellationToken ct)
    {
        var salaries = await repo.GetAllAsync(ct);
        return mapper.Map<IEnumerable<SalaryDto>>(salaries);
    }
}
