using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Exceptions;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Commands.CancelSalesOrder;

public class CancelSalesOrderCommandHandler : IRequestHandler<CancelSalesOrderCommand, SalesOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CancelSalesOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SalesOrderDto> Handle(CancelSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var so = await _unitOfWork.SalesOrders.GetByIdAsync(request.SoId, cancellationToken)
            ?? throw new TransactionNotFoundException("SalesOrder", request.SoId);

        so.Cancel();

        await _unitOfWork.SalesOrders.UpdateAsync(so, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SalesOrderDto>(so);
    }
}
