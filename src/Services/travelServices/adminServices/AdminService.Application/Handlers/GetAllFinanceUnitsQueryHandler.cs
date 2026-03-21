using AutoMapper;
using MediatR;
using AdminService.Application.Queries;
using AdminService.Application.DTOs;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for GetAllFinanceUnitsQuery
/// </summary>
public class GetAllFinanceUnitsQueryHandler : IRequestHandler<GetAllFinanceUnitsQuery, IEnumerable<FinanceUnitDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllFinanceUnitsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<FinanceUnitDto>> Handle(GetAllFinanceUnitsQuery request, CancellationToken cancellationToken)
    {
        var financeUnits = await _unitOfWork.FinanceUnits.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<FinanceUnitDto>>(financeUnits);
    }
}
