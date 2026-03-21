using AgencyService.Application.Common;
using AgencyService.Domain.Repositories;
using MediatR;

namespace AgencyService.Application.Commands;

public class CreateAirlineCommand : ICommand<Result<string>>
{
    public required string Code { get; set; }
    public required string Name { get; set; }
}

public class CreateAirlineCommandHandler : ICommandHandler<CreateAirlineCommand, Result<string>>
{
    private readonly IAirlineRepository _repository;
    
    public CreateAirlineCommandHandler(IAirlineRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<string>> Handle(CreateAirlineCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingAirline = await _repository.GetByCodeAsync(request.Code);
            if (existingAirline != null)
                return Result<string>.FailureResult($"Airline with code {request.Code} already exists");
            
            var airline = new Domain.Entities.Airline(request.Code, request.Name);
            await _repository.AddAsync(airline);
            
            return Result<string>.SuccessResult(airline.Code, "Airline registered successfully");
        }
        catch (Exception ex)
        {
            return Result<string>.FailureResult($"Error registering airline: {ex.Message}");
        }
    }
}
