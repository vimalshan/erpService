using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Queries.GetSalesOrder;

public record GetSalesOrderQuery(int SoId) : IRequest<SalesOrderDto?>;

public class GetSalesOrderQueryHandler : IRequestHandler<GetSalesOrderQuery, SalesOrderDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSalesOrderQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SalesOrderDto?> Handle(GetSalesOrderQuery request, CancellationToken cancellationToken)
    {
        var so = await _unitOfWork.SalesOrders.GetByIdAsync(request.SoId, cancellationToken);
        return so is null ? null : _mapper.Map<SalesOrderDto>(so);
    }
}
