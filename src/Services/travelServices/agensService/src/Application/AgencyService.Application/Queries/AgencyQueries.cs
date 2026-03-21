using AgencyService.Application.Common;
using AgencyService.Application.DTOs;
using AgencyService.Domain.Repositories;
using MediatR;

namespace AgencyService.Application.Queries;

public class GetAgencyByCodeQuery : IQuery<Result<AgencyDto>>
{
    public long AgencyCode { get; set; }
}

public class GetAgencyByCodeQueryHandler : IQueryHandler<GetAgencyByCodeQuery, Result<AgencyDto>>
{
    private readonly IAgencyRepository _repository;
    
    public GetAgencyByCodeQueryHandler(IAgencyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<AgencyDto>> Handle(GetAgencyByCodeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var agency = await _repository.GetByCodeAsync(request.AgencyCode);
            if (agency == null)
                return Result<AgencyDto>.FailureResult($"Agency with code {request.AgencyCode} not found");
            
            var dto = new AgencyDto
            {
                AgencyCode = agency.AgencyCode,
                Name = agency.Name,
                Type = agency.Type.Code,
                Email = agency.ContactInfo.Email,
                Phone = agency.ContactInfo.Phone,
                Address1 = agency.Address.AddressLine1,
                Address2 = agency.Address.AddressLine2,
                CreatedOn = agency.CreatedOn,
                ModifiedOn = agency.ModifiedOn
            };
            
            return Result<AgencyDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return Result<AgencyDto>.FailureResult($"Error retrieving agency: {ex.Message}");
        }
    }
}

public class GetAllAgenciesQuery : IQuery<Result<List<AgencyDto>>>
{
}

public class GetAllAgenciesQueryHandler : IQueryHandler<GetAllAgenciesQuery, Result<List<AgencyDto>>>
{
    private readonly IAgencyRepository _repository;
    
    public GetAllAgenciesQueryHandler(IAgencyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<List<AgencyDto>>> Handle(GetAllAgenciesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var agencies = await _repository.GetAllAsync();
            var dtos = agencies.Select(a => new AgencyDto
            {
                AgencyCode = a.AgencyCode,
                Name = a.Name,
                Type = a.Type.Code,
                Email = a.ContactInfo.Email,
                Phone = a.ContactInfo.Phone,
                Address1 = a.Address.AddressLine1,
                Address2 = a.Address.AddressLine2,
                CreatedOn = a.CreatedOn,
                ModifiedOn = a.ModifiedOn
            }).ToList();
            
            return Result<List<AgencyDto>>.SuccessResult(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<AgencyDto>>.FailureResult($"Error retrieving agencies: {ex.Message}");
        }
    }
}

public class GetAgenciesByTypeQuery : IQuery<Result<List<AgencyDto>>>
{
    public required string Type { get; set; }
}

public class GetAgenciesByTypeQueryHandler : IQueryHandler<GetAgenciesByTypeQuery, Result<List<AgencyDto>>>
{
    private readonly IAgencyRepository _repository;
    
    public GetAgenciesByTypeQueryHandler(IAgencyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<List<AgencyDto>>> Handle(GetAgenciesByTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var agencies = await _repository.GetByTypeAsync(request.Type);
            var dtos = agencies.Select(a => new AgencyDto
            {
                AgencyCode = a.AgencyCode,
                Name = a.Name,
                Type = a.Type.Code,
                Email = a.ContactInfo.Email,
                Phone = a.ContactInfo.Phone,
                Address1 = a.Address.AddressLine1,
                Address2 = a.Address.AddressLine2,
                CreatedOn = a.CreatedOn,
                ModifiedOn = a.ModifiedOn
            }).ToList();
            
            return Result<List<AgencyDto>>.SuccessResult(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<AgencyDto>>.FailureResult($"Error retrieving agencies: {ex.Message}");
        }
    }
}
