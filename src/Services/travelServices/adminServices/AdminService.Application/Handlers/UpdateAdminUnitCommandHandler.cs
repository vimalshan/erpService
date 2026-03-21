using AutoMapper;
using MediatR;
using AdminService.Application.Commands;
using AdminService.Application.DTOs;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for UpdateAdminUnitCommand
/// </summary>
public class UpdateAdminUnitCommandHandler : IRequestHandler<UpdateAdminUnitCommand, AdminUnitDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAdminUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AdminUnitDto> Handle(UpdateAdminUnitCommand request, CancellationToken cancellationToken)
    {
        var adminUnit = await _unitOfWork.AdminUnits.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Admin unit with ID {request.Id} not found");

        adminUnit.Name = request.Name;
        adminUnit.AdminType = request.AdminType;
        adminUnit.UnitCode = request.UnitCode;
        adminUnit.CabUnit = request.CabUnit;
        adminUnit.ImageUrl = request.ImageUrl;
        adminUnit.SortOrder = request.SortOrder;
        adminUnit.ModifiedAt = DateTime.UtcNow;
        adminUnit.ModifiedBy = "SYSTEM";

        var result = await _unitOfWork.AdminUnits.UpdateAsync(adminUnit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AdminUnitDto>(result);
    }
}
