using AutoMapper;
using MediatR;
using AdminService.Application.Queries;
using AdminService.Application.DTOs;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for GetAdminUnitsByTypeQuery
/// </summary>
public class GetAdminUnitsByTypeQueryHandler : IRequestHandler<GetAdminUnitsByTypeQuery, IEnumerable<AdminUnitDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAdminUnitsByTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<AdminUnitDto>> Handle(GetAdminUnitsByTypeQuery request, CancellationToken cancellationToken)
    {
        var adminUnits = await _unitOfWork.AdminUnits.GetByTypeAsync(request.AdminType, cancellationToken);
        return _mapper.Map<IEnumerable<AdminUnitDto>>(adminUnits);
    }
}
