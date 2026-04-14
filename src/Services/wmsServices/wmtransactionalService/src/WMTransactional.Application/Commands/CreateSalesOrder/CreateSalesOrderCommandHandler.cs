using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Entities;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Commands.CreateSalesOrder;

public class CreateSalesOrderCommandHandler : IRequestHandler<CreateSalesOrderCommand, SalesOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSalesOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SalesOrderDto> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var so = new SalesOrder(request.SoNumber, request.CustomerId, request.RequestedDate, request.Notes, request.CreatedBy);

        for (int i = 0; i < request.Lines.Count; i++)
        {
            var line = request.Lines[i];
            so.AddLine(line.ProductId, i + 1, line.QuantityOrdered, line.UnitPrice, line.Notes);
        }

        await _unitOfWork.SalesOrders.AddAsync(so, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SalesOrderDto>(so);
    }
}
