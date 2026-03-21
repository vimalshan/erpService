using AutoMapper;
using MediatR;
using AdminService.Application.Queries;
using AdminService.Application.DTOs;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for GetAllAdminUnitsQuery
/// </summary>
public class GetAllAdminUnitsQueryHandler : IRequestHandler<GetAllAdminUnitsQuery, IEnumerable<AdminUnitDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAdminUnitsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<AdminUnitDto>> Handle(GetAllAdminUnitsQuery request, CancellationToken cancellationToken)
    {
        var adminUnits = await _unitOfWork.AdminUnits.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AdminUnitDto>>(adminUnits);
    }
}
