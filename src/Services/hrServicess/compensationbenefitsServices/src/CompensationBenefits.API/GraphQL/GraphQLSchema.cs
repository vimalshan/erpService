using CompensationBenefits.Application.DTOs;
using CompensationBenefits.Application.Features.Salaries.Queries;
using CompensationBenefits.Application.Features.SalaryStructures;
using CompensationBenefits.Application.Features.Mediclaim;
using HotChocolate;
using MediatR;

namespace CompensationBenefits.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<SalaryDto>> GetSalaries([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllSalariesQuery(), ct);

    public async Task<SalaryDto?> GetSalary(long id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetSalaryByIdQuery(id), ct);

    public async Task<IEnumerable<SalaryStructureDto>> GetSalaryStructures([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllSalaryStructuresQuery(), ct);

    public async Task<SalaryStructureDto?> GetSalaryStructure(long id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetSalaryStructureByIdQuery(id), ct);

    public async Task<IEnumerable<MediclaimDto>> GetMediclaims([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllMediclainsQuery(), ct);

    public async Task<MediclaimDto?> GetMediclaim(long id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetMediclaimByIdQuery(id), ct);
}

public class Mutation
{
    public async Task<long> CreateSalary(
        Application.Features.Salaries.Commands.CreateSalaryCommand command, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(command, ct);

    public async Task<long> CreateSalaryStructure(
        Application.Features.SalaryStructures.CreateSalaryStructureCommand command, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(command, ct);

    public async Task<long> CreateMediclaim(
        Application.Features.Mediclaim.CreateMediclaimCommand command, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(command, ct);
}
