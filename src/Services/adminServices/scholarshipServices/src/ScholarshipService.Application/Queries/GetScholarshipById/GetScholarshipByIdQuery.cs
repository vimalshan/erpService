using AutoMapper;
using MediatR;
using ScholarshipService.Application.DTOs;
using ScholarshipService.Domain.Repositories;

namespace ScholarshipService.Application.Queries.GetScholarshipById;

public record GetScholarshipByIdQuery(int Id) : IRequest<ScholarshipMainDto?>;

public class GetScholarshipByIdQueryHandler(
    IScholarshipMainRepository repository,
    IMapper mapper)
    : IRequestHandler<GetScholarshipByIdQuery, ScholarshipMainDto?>
{
    public async Task<ScholarshipMainDto?> Handle(GetScholarshipByIdQuery request, CancellationToken cancellationToken)
    {
        var scholarship = await repository.GetByIdAsync(request.Id, cancellationToken);
        return scholarship is null ? null : mapper.Map<ScholarshipMainDto>(scholarship);
    }
}
