using AutoMapper;
using MediatR;
using AdminService.Application.Queries;
using AdminService.Application.DTOs;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for GetAdminUnitByIdQuery
/// </summary>
public class GetAdminUnitByIdQueryHandler : IRequestHandler<GetAdminUnitByIdQuery, AdminUnitDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAdminUnitByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AdminUnitDto?> Handle(GetAdminUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var adminUnit = await _unitOfWork.AdminUnits.GetByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<AdminUnitDto?>(adminUnit);
    }
}
