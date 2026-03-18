using TeamServices.Application.DTOs;
using TeamServices.Infrastructure.Data;

namespace TeamServices.API.GraphQL;

public class TeamQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<TeamDto> GetTeams([Service] DapperQueryService dapperService)
    {
        // For GraphQL with filtering/sorting, use EF queryable
        // Dapper used for simple queries; here we return the result as queryable for HotChocolate
        var teams = dapperService.GetAllTeamsAsync().GetAwaiter().GetResult();
        return teams.AsQueryable();
    }

    public async Task<TeamDto?> GetTeamById([Service] DapperQueryService dapperService, long teamId)
    {
        return await dapperService.GetTeamByIdAsync(teamId);
    }

    public async Task<IEnumerable<TeamEmployeeMapDto>> GetTeamEmployees([Service] DapperQueryService dapperService, long teamId)
    {
        return await dapperService.GetTeamEmployeesAsync(teamId);
    }

    public async Task<IEnumerable<TeamUnitMapDto>> GetTeamUnitMaps([Service] DapperQueryService dapperService, long teamId)
    {
        return await dapperService.GetTeamUnitMapsAsync(teamId);
    }
}
