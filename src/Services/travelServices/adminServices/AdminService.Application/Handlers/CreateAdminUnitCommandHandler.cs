using AutoMapper;
using MediatR;
using AdminService.Application.Commands;
using AdminService.Application.DTOs;
using AdminService.Domain.Events;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for CreateAdminUnitCommand
/// </summary>
public class CreateAdminUnitCommandHandler : IRequestHandler<CreateAdminUnitCommand, AdminUnitDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateAdminUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<AdminUnitDto> Handle(CreateAdminUnitCommand request, CancellationToken cancellationToken)
    {
        var adminUnit = new Domain.Entities.AdminUnit
        {
            AdminCode = request.AdminCode,
            Name = request.Name,
            AdminType = request.AdminType,
            UnitCode = request.UnitCode,
            CabUnit = request.CabUnit,
            ImageUrl = request.ImageUrl,
            SortOrder = request.SortOrder,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "SYSTEM"
        };

        var result = await _unitOfWork.AdminUnits.AddAsync(adminUnit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new AdminUnitCreatedEvent(
            result.AdminCode, result.Name, result.AdminType, DateTime.UtcNow), cancellationToken);

        return _mapper.Map<AdminUnitDto>(result);
    }
}
