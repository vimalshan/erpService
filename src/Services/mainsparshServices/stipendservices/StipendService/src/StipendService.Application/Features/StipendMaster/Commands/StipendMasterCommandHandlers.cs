using AutoMapper;
using MediatR;
using StipendService.Application.DTOs;
using StipendService.Domain.Entities;
using StipendService.Domain.Exceptions;
using StipendService.Domain.Interfaces;

namespace StipendService.Application.Features.StipendMaster.Commands;

public class CreateStipendMasterCommandHandler : IRequestHandler<CreateStipendMasterCommand, StipendMasterDto>
{
    private readonly IStipendMasterRepository _repository;
    private readonly IMapper _mapper;

    public CreateStipendMasterCommandHandler(IStipendMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StipendMasterDto> Handle(CreateStipendMasterCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.ResearchCategoryId, request.SrfRankId, cancellationToken);
        if (exists)
            throw new DomainException($"A stipend master already exists for CategoryId={request.ResearchCategoryId}, RankId={request.SrfRankId}.");

        var entity = Domain.Entities.StipendMaster.Create(
            request.ResearchCategoryId,
            request.SrfRankId,
            request.SrfMonthlyStipend,
            request.AdditionalAllowance,
            request.EffectiveFrom,
            request.EffectiveTo,
            request.CreatedBy);

        await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<StipendMasterDto>(entity);
    }
}

public class UpdateStipendMasterCommandHandler : IRequestHandler<UpdateStipendMasterCommand, StipendMasterDto>
{
    private readonly IStipendMasterRepository _repository;
    private readonly IMapper _mapper;

    public UpdateStipendMasterCommandHandler(IStipendMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StipendMasterDto> Handle(UpdateStipendMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.StipendId, cancellationToken)
            ?? throw new KeyNotFoundException($"StipendMaster with id {request.StipendId} not found.");

        entity.Update(request.SrfMonthlyStipend, request.AdditionalAllowance, request.EffectiveFrom, request.EffectiveTo, request.UpdatedBy);
        await _repository.UpdateAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<StipendMasterDto>(entity);
    }
}

public class DeactivateStipendMasterCommandHandler : IRequestHandler<DeactivateStipendMasterCommand, bool>
{
    private readonly IStipendMasterRepository _repository;

    public DeactivateStipendMasterCommandHandler(IStipendMasterRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeactivateStipendMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.StipendId, cancellationToken)
            ?? throw new KeyNotFoundException($"StipendMaster with id {request.StipendId} not found.");

        entity.Deactivate(request.UpdatedBy);
        await _repository.UpdateAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
