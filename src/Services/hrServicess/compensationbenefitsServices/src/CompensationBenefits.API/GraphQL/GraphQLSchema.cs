using CompensationBenefits.Application.DTOs;
using CompensationBenefits.Application.Features.Salaries.Queries;
using CompensationBenefits.Application.Features.SalaryStructures;
using CompensationBenefits.Application.Features.Mediclaim;
using MediatR;

namespace CompensationBenefits.API.GraphQL;

public class Query(IMediator mediator)
{
    public async Task<IEnumerable<SalaryDto>> GetSalaries(CancellationToken ct)
        => await mediator.Send(new GetAllSalariesQuery(), ct);

    public async Task<SalaryDto?> GetSalary(long id, CancellationToken ct)
        => await mediator.Send(new GetSalaryByIdQuery(id), ct);

    public async Task<IEnumerable<SalaryStructureDto>> GetSalaryStructures(CancellationToken ct)
        => await mediator.Send(new GetAllSalaryStructuresQuery(), ct);

    public async Task<SalaryStructureDto?> GetSalaryStructure(long id, CancellationToken ct)
        => await mediator.Send(new GetSalaryStructureByIdQuery(id), ct);

    public async Task<IEnumerable<MediclaimDto>> GetMediclaims(CancellationToken ct)
        => await mediator.Send(new GetAllMediclainsQuery(), ct);

    public async Task<MediclaimDto?> GetMediclaim(long id, CancellationToken ct)
        => await mediator.Send(new GetMediclaimByIdQuery(id), ct);
}

public class Mutation(IMediator mediator)
{
    public async Task<long> CreateSalary(
        Application.Features.Salaries.Commands.CreateSalaryCommand command, CancellationToken ct)
        => await mediator.Send(command, ct);

    public async Task<long> CreateSalaryStructure(
        Application.Features.SalaryStructures.CreateSalaryStructureCommand command, CancellationToken ct)
        => await mediator.Send(command, ct);

    public async Task<long> CreateMediclaim(
        Application.Features.Mediclaim.CreateMediclaimCommand command, CancellationToken ct)
        => await mediator.Send(command, ct);
}
