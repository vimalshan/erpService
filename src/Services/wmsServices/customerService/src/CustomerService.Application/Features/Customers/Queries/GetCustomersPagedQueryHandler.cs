using AutoMapper;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Interfaces;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries;

public sealed class GetCustomersPagedQueryHandler : IRequestHandler<GetCustomersPagedQuery, PagedResultDto<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCustomersPagedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<CustomerDto>> Handle(GetCustomersPagedQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _unitOfWork.Customers.GetPagedAsync(
            request.Page, request.PageSize, request.Search, cancellationToken);

        return new PagedResultDto<CustomerDto>
        {
            Items = _mapper.Map<IReadOnlyList<CustomerDto>>(items),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
