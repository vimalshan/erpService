using MediatR;
using TeamServices.Application.Commands;
using TeamServices.Application.DTOs;

namespace TeamServices.API.GraphQL;

public class TeamMutation
{
    public async Task<TeamDto> CreateTeam([Service] IMediator mediator, long teamId, string teamName, long modifiedBy)
    {
        return await mediator.Send(new CreateTeamCommand(teamId, teamName, modifiedBy));
    }

    public async Task<TeamDto> UpdateTeam([Service] IMediator mediator, long teamId, string teamName, long modifiedBy)
    {
        return await mediator.Send(new UpdateTeamCommand(teamId, teamName, modifiedBy));
    }

    public async Task<bool> DeleteTeam([Service] IMediator mediator, long teamId)
    {
        await mediator.Send(new DeleteTeamCommand(teamId));
        return true;
    }

    public async Task<TeamEmployeeMapDto> AddTeamEmployee([Service] IMediator mediator,
        long id, long teamId, long employeeSysId, DateTime effectiveDate, DateTime? closeDate, long modifiedBy)
    {
        return await mediator.Send(new AddTeamEmployeeCommand(id, teamId, employeeSysId, effectiveDate, closeDate, modifiedBy));
    }

    public async Task<TeamUnitMapDto> AddTeamUnitMap([Service] IMediator mediator,
        long mapId, long teamId, long unitId, char gradeCategory, long? cadreId, long modifiedBy)
    {
        return await mediator.Send(new AddTeamUnitMapCommand(mapId, teamId, unitId, gradeCategory, cadreId, modifiedBy));
    }
}
