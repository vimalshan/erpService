using AutoMapper;
using MediatR;
using AdminService.Application.Commands;
using AdminService.Application.DTOs;
using AdminService.Domain.Events;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for CreateFinanceUnitCommand
/// </summary>
public class CreateFinanceUnitCommandHandler : IRequestHandler<CreateFinanceUnitCommand, FinanceUnitDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateFinanceUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<FinanceUnitDto> Handle(CreateFinanceUnitCommand request, CancellationToken cancellationToken)
    {
        var financeUnit = new Domain.Entities.FinanceUnit
        {
            UnitId = request.UnitId,
            UnitCode = request.UnitCode,
            Name = request.Name,
            OracleCode = request.OracleCode,
            LocationOption = "N",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "SYSTEM"
        };

        var result = await _unitOfWork.FinanceUnits.AddAsync(financeUnit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new FinanceUnitCreatedEvent(
            result.UnitId, result.UnitCode ?? string.Empty, result.Name, DateTime.UtcNow), cancellationToken);

        return _mapper.Map<FinanceUnitDto>(result);
    }
}
