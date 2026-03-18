using AutoMapper;
using MediatR;
using EligibilityService.Application.Commands.DaywiseEligibility;
using EligibilityService.Application.DTOs;
using EligibilityService.Domain.Interfaces;

namespace EligibilityService.Application.Commands.DaywiseEligibility;

public class CreateDaywiseEligibilityHandler : IRequestHandler<CreateDaywiseEligibilityCommand, DaywiseEligibilityDto>
{
    private readonly IDaywiseEligibilityRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateDaywiseEligibilityHandler(IDaywiseEligibilityRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<DaywiseEligibilityDto> Handle(CreateDaywiseEligibilityCommand request, CancellationToken cancellationToken)
    {
        var entity = Domain.Entities.DaywiseEligibility.Create(
            request.SerialNumber, request.CompanyCode, request.EmployeeSysId,
            request.AttendanceDate, request.ProcessNumber, request.ShiftCode,
            request.ItemCode, request.ShiftQuantity, request.BeforeShiftQty,
            request.AfterShiftQty, request.EnteredUser, request.FlexField1, request.GradeType);

        await _repo.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DaywiseEligibilityDto>(entity);
    }
}

public class DeleteDaywiseEligibilityHandler : IRequestHandler<DeleteDaywiseEligibilityCommand, bool>
{
    private readonly IDaywiseEligibilityRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteDaywiseEligibilityHandler(IDaywiseEligibilityRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteDaywiseEligibilityCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetBySerialNumberAsync(request.SerialNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"DaywiseEligibility with serial {request.SerialNumber} not found.");

        _repo.Remove(entity);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
