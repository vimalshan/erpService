using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Exceptions;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Commands.ConfirmSalesOrder;

public class ConfirmSalesOrderCommandHandler : IRequestHandler<ConfirmSalesOrderCommand, SalesOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConfirmSalesOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SalesOrderDto> Handle(ConfirmSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var so = await _unitOfWork.SalesOrders.GetByIdAsync(request.SoId, cancellationToken)
            ?? throw new TransactionNotFoundException("SalesOrder", request.SoId);

        so.Confirm();

        await _unitOfWork.SalesOrders.UpdateAsync(so, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SalesOrderDto>(so);
    }
}
