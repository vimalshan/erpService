using AutoMapper;
using MediatR;
using SettlementService.Application.DTOs;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Application.Queries.GetSettlements;

public class GetSettlementsQueryHandler : IRequestHandler<GetSettlementsQuery, IEnumerable<SettlementDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSettlementsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SettlementDto>> Handle(GetSettlementsQuery request, CancellationToken cancellationToken)
    {
        var settlements = await _unitOfWork.Settlements.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<SettlementDto>>(settlements);
    }
}
