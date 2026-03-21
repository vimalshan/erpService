using AgencyService.Application.Common;
using AgencyService.Domain.Repositories;
using MediatR;

namespace AgencyService.Application.Commands;

public class CreateVendorCommand : ICommand<Result<long>>
{
    public long VendorId { get; set; }
    public required string Name { get; set; }
    public required string CategoryType { get; set; }
    public string? Phone { get; set; }
    public string? Address1 { get; set; }
}

public class CreateVendorCommandHandler : ICommandHandler<CreateVendorCommand, Result<long>>
{
    private readonly IVendorRepository _repository;
    
    public CreateVendorCommandHandler(IVendorRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<long>> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingVendor = await _repository.GetByIdAsync(request.VendorId);
            if (existingVendor != null)
                return Result<long>.FailureResult($"Vendor with ID {request.VendorId} already exists");
            
            var vendor = new Domain.Entities.Vendor(
                request.VendorId,
                request.Name,
                request.CategoryType,
                request.Phone,
                request.Address1);
            
            await _repository.AddAsync(vendor);
            
            return Result<long>.SuccessResult(vendor.Id, "Vendor created successfully");
        }
        catch (Exception ex)
        {
            return Result<long>.FailureResult($"Error creating vendor: {ex.Message}");
        }
    }
}

public class UpdateVendorCommand : ICommand<Result>
{
    public long VendorId { get; set; }
    public required string Name { get; set; }
    public string? Phone { get; set; }
    public long? CityCode { get; set; }
}

public class UpdateVendorCommandHandler : ICommandHandler<UpdateVendorCommand, Result>
{
    private readonly IVendorRepository _repository;
    
    public UpdateVendorCommandHandler(IVendorRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vendor = await _repository.GetByIdAsync(request.VendorId);
            if (vendor == null)
                return Result.FailureResult($"Vendor with ID {request.VendorId} not found");
            
            vendor.Update(request.Name, request.Phone, request.CityCode);
            await _repository.UpdateAsync(vendor);
            
            return Result.SuccessResult("Vendor updated successfully");
        }
        catch (Exception ex)
        {
            return Result.FailureResult($"Error updating vendor: {ex.Message}");
        }
    }
}

public class DeleteVendorCommand : ICommand<Result>
{
    public long VendorId { get; set; }
}

public class DeleteVendorCommandHandler : ICommandHandler<DeleteVendorCommand, Result>
{
    private readonly IVendorRepository _repository;
    
    public DeleteVendorCommandHandler(IVendorRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vendor = await _repository.GetByIdAsync(request.VendorId);
            if (vendor == null)
                return Result.FailureResult($"Vendor with ID {request.VendorId} not found");
            
            await _repository.DeleteAsync(request.VendorId);
            
            return Result.SuccessResult("Vendor deleted successfully");
        }
        catch (Exception ex)
        {
            return Result.FailureResult($"Error deleting vendor: {ex.Message}");
        }
    }
}
