using MediatR;
using CompetencyService.Application.DTOs;

namespace CompetencyService.Application.Commands.Competencies;

public record CreateCompetencyCommand(
    decimal Id,
    string Name,
    DateTime EffectiveDate,
    string? CompetencyType,
    decimal? ParentId,
    string? Remarks,
    decimal? JobCode,
    string? PositiveIndicator,
    string? NegativeIndicator,
    string? SelfDescription
) : IRequest<CompetencyDto>;

public record UpdateCompetencyCommand(
    decimal Id,
    string Name,
    DateTime EffectiveDate,
    DateTime? ClosureDate,
    string? Remarks,
    string? CompetencyType,
    decimal? ModifiedBy
) : IRequest<CompetencyDto>;

public record CloseCompetencyCommand(decimal Id, DateTime ClosureDate, decimal? ModifiedBy)
    : IRequest<bool>;

public record DeleteCompetencyCommand(decimal Id) : IRequest<bool>;
