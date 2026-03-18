using AutoMapper;
using MediatR;
using SettlementService.Application.DTOs;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Application.Queries.GetSettlement;

public class GetSettlementQueryHandler : IRequestHandler<GetSettlementQuery, SettlementDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSettlementQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SettlementDto?> Handle(GetSettlementQuery request, CancellationToken cancellationToken)
    {
        var settlement = await _unitOfWork.Settlements.GetByIdAsync(request.SettlementNumber, cancellationToken);
        return settlement is null ? null : _mapper.Map<SettlementDto>(settlement);
    }
}
