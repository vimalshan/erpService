using MasterDataService.Application.DTOs;
using MasterDataService.Application.Interfaces;
using MasterDataService.Domain.Interfaces;
using MasterDataService.Application.Features.RateMaster.Queries;
using MasterDataService.Application.Features.RateMaster.Commands;
using MediatR;

namespace MasterDataService.Application.Features.RateMaster;

public class GetAllRatesQueryHandler : IRequestHandler<GetAllRatesQuery, IEnumerable<RateMasterDto>>
{
    private readonly IRateMasterRepository _repository;
    public GetAllRatesQueryHandler(IRateMasterRepository repository) => _repository = repository;

    public async Task<IEnumerable<RateMasterDto>> Handle(GetAllRatesQuery request, CancellationToken cancellationToken)
    {
        var entities = string.IsNullOrEmpty(request.TrustCode)
            ? await _repository.GetAllAsync(cancellationToken)
            : await _repository.GetByTrustCodeAsync(request.TrustCode, cancellationToken);
        return entities.Select(e => new RateMasterDto(e.TrustCode, e.RateId, e.RateTypeCode, e.RateEffectiveDate, e.RateClosingDate, e.RateValue, e.RateDeleteFlag, e.ReworkStatus));
    }
}

public class CreateRateCommandHandler : IRequestHandler<CreateRateCommand, RateMasterDto>
{
    private readonly IRateMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateRateCommandHandler(IRateMasterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RateMasterDto> Handle(CreateRateCommand request, CancellationToken cancellationToken)
    {
        var nextId = await _repository.GetNextRateIdAsync(request.TrustCode, cancellationToken);
        var entity = new Domain.Entities.RateMaster
        {
            TrustCode = request.TrustCode,
            RateId = nextId,
            RateTypeCode = request.RateTypeCode,
            RateEffectiveDate = request.RateEffectiveDate,
            RateValue = request.RateValue
        };
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new RateMasterDto(entity.TrustCode, entity.RateId, entity.RateTypeCode, entity.RateEffectiveDate, entity.RateClosingDate, entity.RateValue, entity.RateDeleteFlag, entity.ReworkStatus);
    }
}
