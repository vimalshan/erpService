using EmployeePrideManagement.Application.DTOs;
using EmployeePrideManagement.Domain.Interfaces;
using MediatR;

namespace EmployeePrideManagement.Application.Queries.GetAllPrideMoments;

public class GetAllPrideMomentsQueryHandler : IRequestHandler<GetAllPrideMomentsQuery, PagedResultDto<PrideMomentDto>>
{
    private readonly IDapperPrideMomentRepository _repository;

    public GetAllPrideMomentsQueryHandler(IDapperPrideMomentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<PrideMomentDto>> Handle(GetAllPrideMomentsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.GetAllPagedAsync<PrideMomentDto>(request.PageNumber, request.PageSize);

        return new PagedResultDto<PrideMomentDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
