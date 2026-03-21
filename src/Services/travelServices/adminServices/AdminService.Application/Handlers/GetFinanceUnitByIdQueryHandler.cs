using AutoMapper;
using MediatR;
using AdminService.Application.Queries;
using AdminService.Application.DTOs;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for GetFinanceUnitByIdQuery
/// </summary>
public class GetFinanceUnitByIdQueryHandler : IRequestHandler<GetFinanceUnitByIdQuery, FinanceUnitDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFinanceUnitByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<FinanceUnitDto?> Handle(GetFinanceUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var financeUnit = await _unitOfWork.FinanceUnits.GetByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<FinanceUnitDto?>(financeUnit);
    }
}
