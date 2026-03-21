using AgencyService.Application.Common;
using AgencyService.Domain.Repositories;
using AgencyService.Domain.ValueObjects;
using MediatR;

namespace AgencyService.Application.Commands;

public class CreateAgencyCommand : ICommand<Result<long>>
{
    public long AgencyCode { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }
    public string? Address4 { get; set; }
}

public class CreateAgencyCommandHandler : ICommandHandler<CreateAgencyCommand, Result<long>>
{
    private readonly IAgencyRepository _repository;
    
    public CreateAgencyCommandHandler(IAgencyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<long>> Handle(CreateAgencyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingAgency = await _repository.GetByCodeAsync(request.AgencyCode);
            if (existingAgency != null)
                return Result<long>.FailureResult($"Agency with code {request.AgencyCode} already exists");
            
            var agencyType = AgencyType.Create(request.Type);
            var contactInfo = new ContactInfo(request.Email, request.Phone);
            var address = new Address(request.Address1, request.Address2, request.Address3, request.Address4);
            
            var agency = new Domain.Entities.Agency(
                request.AgencyCode,
                request.Name,
                agencyType,
                contactInfo,
                address);
            
            await _repository.AddAsync(agency);
            
            return Result<long>.SuccessResult(agency.AgencyCode, "Agency created successfully");
        }
        catch (Exception ex)
        {
            return Result<long>.FailureResult($"Error creating agency: {ex.Message}");
        }
    }
}

public class UpdateAgencyCommand : ICommand<Result>
{
    public long AgencyCode { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public long ModifiedBy { get; set; }
}

public class UpdateAgencyCommandHandler : ICommandHandler<UpdateAgencyCommand, Result>
{
    private readonly IAgencyRepository _repository;
    
    public UpdateAgencyCommandHandler(IAgencyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(UpdateAgencyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var agency = await _repository.GetByCodeAsync(request.AgencyCode);
            if (agency == null)
                return Result.FailureResult($"Agency with code {request.AgencyCode} not found");
            
            var contactInfo = new ContactInfo(request.Email, request.Phone);
            agency.Update(request.Name, contactInfo, request.ModifiedBy);
            
            await _repository.UpdateAsync(agency);
            
            return Result.SuccessResult("Agency updated successfully");
        }
        catch (Exception ex)
        {
            return Result.FailureResult($"Error updating agency: {ex.Message}");
        }
    }
}

public class DeleteAgencyCommand : ICommand<Result>
{
    public long AgencyCode { get; set; }
}

public class DeleteAgencyCommandHandler : ICommandHandler<DeleteAgencyCommand, Result>
{
    private readonly IAgencyRepository _repository;
    
    public DeleteAgencyCommandHandler(IAgencyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(DeleteAgencyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var agency = await _repository.GetByCodeAsync(request.AgencyCode);
            if (agency == null)
                return Result.FailureResult($"Agency with code {request.AgencyCode} not found");
            
            await _repository.DeleteAsync(request.AgencyCode);
            
            return Result.SuccessResult("Agency deleted successfully");
        }
        catch (Exception ex)
        {
            return Result.FailureResult($"Error deleting agency: {ex.Message}");
        }
    }
}
