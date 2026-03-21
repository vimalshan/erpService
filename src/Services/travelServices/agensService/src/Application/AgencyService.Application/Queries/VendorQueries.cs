using AgencyService.Application.Common;
using AgencyService.Application.DTOs;
using AgencyService.Domain.Repositories;
using MediatR;

namespace AgencyService.Application.Queries;

public class GetVendorByIdQuery : IQuery<Result<VendorDto>>
{
    public long VendorId { get; set; }
}

public class GetVendorByIdQueryHandler : IQueryHandler<GetVendorByIdQuery, Result<VendorDto>>
{
    private readonly IVendorRepository _repository;
    
    public GetVendorByIdQueryHandler(IVendorRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<VendorDto>> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var vendor = await _repository.GetByIdAsync(request.VendorId);
            if (vendor == null)
                return Result<VendorDto>.FailureResult($"Vendor with ID {request.VendorId} not found");
            
            var dto = new VendorDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                CategoryType = vendor.CategoryType,
                Phone = vendor.Phone,
                Address = vendor.AddressLine1,
                BankName = vendor.BankName,
                AccountNumber = vendor.AccountNumber
            };
            
            return Result<VendorDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return Result<VendorDto>.FailureResult($"Error retrieving vendor: {ex.Message}");
        }
    }
}

public class GetAllVendorsQuery : IQuery<Result<List<VendorDto>>>
{
}

public class GetAllVendorsQueryHandler : IQueryHandler<GetAllVendorsQuery, Result<List<VendorDto>>>
{
    private readonly IVendorRepository _repository;
    
    public GetAllVendorsQueryHandler(IVendorRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<List<VendorDto>>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var vendors = await _repository.GetAllAsync();
            var dtos = vendors.Select(v => new VendorDto
            {
                Id = v.Id,
                Name = v.Name,
                CategoryType = v.CategoryType,
                Phone = v.Phone,
                Address = v.AddressLine1,
                BankName = v.BankName,
                AccountNumber = v.AccountNumber
            }).ToList();
            
            return Result<List<VendorDto>>.SuccessResult(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<VendorDto>>.FailureResult($"Error retrieving vendors: {ex.Message}");
        }
    }
}

public class GetVendorsByCategoryQuery : IQuery<Result<List<VendorDto>>>
{
    public required string CategoryType { get; set; }
}

public class GetVendorsByCategoryQueryHandler : IQueryHandler<GetVendorsByCategoryQuery, Result<List<VendorDto>>>
{
    private readonly IVendorRepository _repository;
    
    public GetVendorsByCategoryQueryHandler(IVendorRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<List<VendorDto>>> Handle(GetVendorsByCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var vendors = await _repository.GetByCategoryAsync(request.CategoryType);
            var dtos = vendors.Select(v => new VendorDto
            {
                Id = v.Id,
                Name = v.Name,
                CategoryType = v.CategoryType,
                Phone = v.Phone,
                Address = v.AddressLine1,
                BankName = v.BankName,
                AccountNumber = v.AccountNumber
            }).ToList();
            
            return Result<List<VendorDto>>.SuccessResult(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<VendorDto>>.FailureResult($"Error retrieving vendors: {ex.Message}");
        }
    }
}
