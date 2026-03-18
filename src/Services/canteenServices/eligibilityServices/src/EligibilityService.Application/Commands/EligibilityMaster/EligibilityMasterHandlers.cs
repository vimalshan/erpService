using AutoMapper;
using MediatR;
using EligibilityService.Application.Commands.EligibilityMaster;
using EligibilityService.Application.DTOs;
using EligibilityService.Domain.Aggregates;
using EligibilityService.Domain.Exceptions;
using EligibilityService.Domain.Interfaces;

namespace EligibilityService.Application.Commands.EligibilityMaster;

public class CreateEligibilityMasterHandler : IRequestHandler<CreateEligibilityMasterCommand, EligibilityMasterDto>
{
    private readonly IEligibilityMasterRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateEligibilityMasterHandler(IEligibilityMasterRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EligibilityMasterDto> Handle(CreateEligibilityMasterCommand request, CancellationToken cancellationToken)
    {
        if (await _repo.ExistsAsync(request.CanteenUnit, request.ShiftCode, request.ItemCode, cancellationToken))
            throw new DuplicateEligibilityException(request.CanteenUnit, request.ShiftCode, request.ItemCode);

        var aggregate = EligibilityAggregate.CreateNew(
            request.CanteenUnit, request.ShiftCode, request.ItemCode,
            request.EligibleLimit, request.EnteredUser, request.TimeOfficeUnit);

        await _repo.AddAsync(aggregate.Master, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EligibilityMasterDto>(aggregate.Master);
    }
}

public class UpdateEligibilityMasterHandler : IRequestHandler<UpdateEligibilityMasterCommand, EligibilityMasterDto>
{
    private readonly IEligibilityMasterRepository _repo;
    private readonly IEligibilityMasterHistoryRepository _histRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateEligibilityMasterHandler(
        IEligibilityMasterRepository repo,
        IEligibilityMasterHistoryRepository histRepo,
        IUnitOfWork uow,
        IMapper mapper)
    {
        _repo = repo;
        _histRepo = histRepo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EligibilityMasterDto> Handle(UpdateEligibilityMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetAsync(request.CanteenUnit, request.ShiftCode, request.ItemCode, cancellationToken)
            ?? throw new EligibilityNotFoundException(request.CanteenUnit, request.ShiftCode, request.ItemCode);

        var aggregate = EligibilityAggregate.Load(entity);
        aggregate.UpdateLimit(request.EligibleLimit, request.TimeOfficeUnit, request.ModifiedUser);

        foreach (var snapshot in aggregate.HistorySnapshots)
            await _histRepo.AddAsync(snapshot, cancellationToken);

        _repo.Update(aggregate.Master);
        await _uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EligibilityMasterDto>(aggregate.Master);
    }
}

public class DeleteEligibilityMasterHandler : IRequestHandler<DeleteEligibilityMasterCommand, bool>
{
    private readonly IEligibilityMasterRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteEligibilityMasterHandler(IEligibilityMasterRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteEligibilityMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetAsync(request.CanteenUnit, request.ShiftCode, request.ItemCode, cancellationToken)
            ?? throw new EligibilityNotFoundException(request.CanteenUnit, request.ShiftCode, request.ItemCode);

        _repo.Remove(entity);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
