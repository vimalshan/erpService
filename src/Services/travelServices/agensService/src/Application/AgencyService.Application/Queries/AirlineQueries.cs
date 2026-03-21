using AgencyService.Application.Common;
using AgencyService.Application.DTOs;
using AgencyService.Domain.Repositories;
using MediatR;

namespace AgencyService.Application.Queries;

public class GetAirlineByCodeQuery : IQuery<Result<AirlineDto>>
{
    public required string Code { get; set; }
}

public class GetAirlineByCodeQueryHandler : IQueryHandler<GetAirlineByCodeQuery, Result<AirlineDto>>
{
    private readonly IAirlineRepository _repository;
    
    public GetAirlineByCodeQueryHandler(IAirlineRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<AirlineDto>> Handle(GetAirlineByCodeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var airline = await _repository.GetByCodeAsync(request.Code);
            if (airline == null)
                return Result<AirlineDto>.FailureResult($"Airline with code {request.Code} not found");
            
            var dto = new AirlineDto
            {
                Code = airline.Code,
                Name = airline.Name
            };
            
            return Result<AirlineDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return Result<AirlineDto>.FailureResult($"Error retrieving airline: {ex.Message}");
        }
    }
}

public class GetAllAirlinesQuery : IQuery<Result<List<AirlineDto>>>
{
}

public class GetAllAirlinesQueryHandler : IQueryHandler<GetAllAirlinesQuery, Result<List<AirlineDto>>>
{
    private readonly IAirlineRepository _repository;
    
    public GetAllAirlinesQueryHandler(IAirlineRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<List<AirlineDto>>> Handle(GetAllAirlinesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var airlines = await _repository.GetAllAsync();
            var dtos = airlines.Select(a => new AirlineDto
            {
                Code = a.Code,
                Name = a.Name
            }).ToList();
            
            return Result<List<AirlineDto>>.SuccessResult(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<AirlineDto>>.FailureResult($"Error retrieving airlines: {ex.Message}");
        }
    }
}
