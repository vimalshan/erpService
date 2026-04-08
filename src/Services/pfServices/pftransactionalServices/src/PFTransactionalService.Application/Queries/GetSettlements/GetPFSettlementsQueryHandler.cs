using AutoMapper;
using MediatR;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Domain.Interfaces;

namespace PFTransactionalService.Application.Queries.GetSettlements;

public class GetPFSettlementsQueryHandler : IRequestHandler<GetPFSettlementsQuery, IEnumerable<PFSettlementDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPFSettlementsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PFSettlementDto>> Handle(GetPFSettlementsQuery request, CancellationToken cancellationToken)
    {
        var settlements = await _unitOfWork.Settlements.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PFSettlementDto>>(settlements);
    }
}

public class GetPFSettlementsByEmpQueryHandler : IRequestHandler<GetPFSettlementsByEmpQuery, IEnumerable<PFSettlementDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPFSettlementsByEmpQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PFSettlementDto>> Handle(GetPFSettlementsByEmpQuery request, CancellationToken cancellationToken)
    {
        var settlements = await _unitOfWork.Settlements.GetByEmpSysIdAsync(request.EmpSysId, cancellationToken);
        return _mapper.Map<IEnumerable<PFSettlementDto>>(settlements);
    }
}
