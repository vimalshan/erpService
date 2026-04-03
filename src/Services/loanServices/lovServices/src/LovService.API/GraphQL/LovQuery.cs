using LovService.Application.DTOs;
using LovService.Application.Features.LovMaster.Queries;
using LovService.Application.Features.LovTypeMast.Queries;
using LovService.Application.Features.ProgramLovMast.Queries;
using MediatR;

namespace LovService.API.GraphQL;

public class LovQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<LovTypeMastDto>> GetLovTypes([Service] IMediator mediator)
        => await mediator.Send(new GetAllLovTypesQuery());

    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<LovMasterDto>> GetLovMasters([Service] IMediator mediator)
        => await mediator.Send(new GetAllLovMastersQuery());

    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<ProgramLovMastDto>> GetProgramLovs([Service] IMediator mediator)
        => await mediator.Send(new GetAllProgramLovsQuery());
}
