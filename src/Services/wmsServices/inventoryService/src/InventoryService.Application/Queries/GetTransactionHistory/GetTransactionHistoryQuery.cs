using AutoMapper;
using MediatR;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Application.Queries.GetTransactionHistory;

public record GetTransactionHistoryQuery : IRequest<IEnumerable<InventoryTransactionDto>>
{
    public int? ProductId { get; init; }
    public int? WarehouseId { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
}

public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, IEnumerable<InventoryTransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTransactionHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryTransactionDto>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.InventoryTransaction> transactions;

        if (request.FromDate.HasValue && request.ToDate.HasValue)
        {
            transactions = await _unitOfWork.InventoryTransactions
                .GetByDateRangeAsync(request.FromDate.Value, request.ToDate.Value, cancellationToken);
        }
        else if (request.ProductId.HasValue)
        {
            transactions = await _unitOfWork.InventoryTransactions
                .GetByProductAsync(request.ProductId.Value, cancellationToken);
        }
        else if (request.WarehouseId.HasValue)
        {
            transactions = await _unitOfWork.InventoryTransactions
                .GetByWarehouseAsync(request.WarehouseId.Value, cancellationToken);
        }
        else
        {
            transactions = await _unitOfWork.InventoryTransactions
                .GetByDateRangeAsync(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, cancellationToken);
        }

        return _mapper.Map<IEnumerable<InventoryTransactionDto>>(transactions);
    }
}
