using HotChocolate;
using HotChocolate.Types;
using MediatR;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Application.Queries.Applications;
using RecruitmentService.Application.Queries.Prospects;
using RecruitmentService.Application.Queries.Vacancies;

namespace RecruitmentService.API.GraphQL;

public class RecruitmentQuery
{
    /// <summary>Retrieve all open vacancies.</summary>
    public async Task<IEnumerable<VacancySummaryDto>> GetVacancies(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllVacanciesQuery(), ct);

    /// <summary>Retrieve a single vacancy by ID.</summary>
    public async Task<VacancyDto> GetVacancy(
        decimal id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetVacancyByIdQuery(id), ct);

    /// <summary>Retrieve an application by ID.</summary>
    public async Task<ApplicationDto> GetApplication(
        decimal id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetApplicationByIdQuery(id), ct);

    /// <summary>Retrieve all applications for a vacancy.</summary>
    public async Task<IEnumerable<ApplicationDto>> GetApplicationsByVacancy(
        decimal vacancyId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetApplicationsByVacancyQuery(vacancyId), ct);

    /// <summary>Retrieve a prospect by user ID.</summary>
    public async Task<ProspectDto> GetProspect(
        decimal userId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetProspectByIdQuery(userId), ct);

    /// <summary>Retrieve all prospects.</summary>
    public async Task<IEnumerable<ProspectSummaryDto>> GetProspects(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllProspectsQuery(), ct);
}
