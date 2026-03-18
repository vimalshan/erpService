using GroupIncentiveService.Application.DTOs;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Queries.GetEmployeeIncentive;

public record GetEmployeeIncentiveQuery(long EmployeeId, int Month, int Year) : IRequest<EmployeeIncentiveSummaryDto>;

public class GetEmployeeIncentiveHandler : IRequestHandler<GetEmployeeIncentiveQuery, EmployeeIncentiveSummaryDto>
{
    private readonly IGroupIncentiveDetRepository _detRepo;

    public GetEmployeeIncentiveHandler(IGroupIncentiveDetRepository detRepo)
    {
        _detRepo = detRepo;
    }

    public async Task<EmployeeIncentiveSummaryDto> Handle(GetEmployeeIncentiveQuery request, CancellationToken cancellationToken)
    {
        // This would be implemented using a Dapper query for performance
        // The repository interface would need a specialized method
        // For now return a stub that the Dapper repo implements
        return new EmployeeIncentiveSummaryDto(request.EmployeeId, request.Month, request.Year, 0m, 0m);
    }
}
