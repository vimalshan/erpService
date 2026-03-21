using AutoMapper;
using InsuranceService.Application.DTOs;
using InsuranceService.Domain.Repositories;
using MediatR;

namespace InsuranceService.Application.Queries;

public class GetInsuranceDetailsQueryHandler : IRequestHandler<GetInsuranceDetailsQuery, IReadOnlyList<TravelInsuranceDto>>
{
    private readonly ITravelInsuranceRepository _repository;
    private readonly IMapper _mapper;

    public GetInsuranceDetailsQueryHandler(ITravelInsuranceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TravelInsuranceDto>> Handle(GetInsuranceDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.PlanNumber.HasValue && !string.IsNullOrEmpty(request.CompanyCode))
        {
            var single = await _repository.GetByKeyAsync(request.CompanyCode, request.PlanNumber.Value, cancellationToken);
            if (single is null) return [];

            return [_mapper.Map<TravelInsuranceDto>(single)];
        }

        var results = await _repository.GetAllAsync(request.CompanyCode, cancellationToken);
        return _mapper.Map<IReadOnlyList<TravelInsuranceDto>>(results);
    }
}
