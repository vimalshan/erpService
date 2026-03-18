using AutoMapper;
using MediatR;
using ScholarshipService.Application.Common;
using ScholarshipService.Application.DTOs;
using ScholarshipService.Domain.Repositories;

namespace ScholarshipService.Application.Queries.GetScholarships;

public record GetScholarshipsQuery(int? EmployeeSysId = null, int Page = 1, int PageSize = 20) : IRequest<PagedResult<ScholarshipMainDto>>;

public class GetScholarshipsQueryHandler(
    IScholarshipMainRepository repository,
    IMapper mapper)
    : IRequestHandler<GetScholarshipsQuery, PagedResult<ScholarshipMainDto>>
{
    public async Task<PagedResult<ScholarshipMainDto>> Handle(GetScholarshipsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.ScholarshipMain> scholarships;

        if (request.EmployeeSysId.HasValue)
            scholarships = await repository.GetByEmployeeIdAsync(request.EmployeeSysId.Value, cancellationToken);
        else
            scholarships = await repository.GetAllAsync(cancellationToken);

        var list = scholarships.ToList();
        var total = list.Count;
        var items = list
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(mapper.Map<ScholarshipMainDto>)
            .ToList();

        return new PagedResult<ScholarshipMainDto>
        {
            Items = items,
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
