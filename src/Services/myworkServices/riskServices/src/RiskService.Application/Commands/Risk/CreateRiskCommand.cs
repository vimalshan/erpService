using MediatR;
using RiskService.Application.DTOs;

namespace RiskService.Application.Commands.Risk;

public record CreateRiskCommand : IRequest<long>
{
    public char ApplicableTo { get; init; }
    public long OrganizationId { get; init; }
    public long BusinessId { get; init; }
    public long DivisionId { get; init; }
    public long UnitId { get; init; }
    public long FunctionId { get; init; }
    public string EventTitle { get; init; } = default!;
    public string Description { get; init; } = default!;
    public long TypeId { get; init; }
    public long ImpactId { get; init; }
    public long ProbabilityId { get; init; }
    public long RatingId { get; init; }
    public long ResidualImpactId { get; init; }
    public long ResidualProbabilityId { get; init; }
    public long ResidualRatingId { get; init; }
    public long ResponseId { get; init; }
    public long OwnerId { get; init; }
    public long CreatedBy { get; init; }
    public List<CreateRiskCauseDto> Causes { get; init; } = new();
    public List<CreateRiskControlDto> Controls { get; init; } = new();
}

public record CreateRiskCauseDto(string Description);
public record CreateRiskControlDto(string Description, string FileName);
