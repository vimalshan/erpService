using AutoMapper;
using MediatR;
using SettlementService.Application.DTOs;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Application.Queries.GetSettlementsByMember;

public class GetSettlementsByMemberQueryHandler : IRequestHandler<GetSettlementsByMemberQuery, IEnumerable<SettlementDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSettlementsByMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SettlementDto>> Handle(GetSettlementsByMemberQuery request, CancellationToken cancellationToken)
    {
        var settlements = await _unitOfWork.Settlements.GetByMemberNoAsync(request.MemberNo, cancellationToken);
        return _mapper.Map<IEnumerable<SettlementDto>>(settlements);
    }
}
