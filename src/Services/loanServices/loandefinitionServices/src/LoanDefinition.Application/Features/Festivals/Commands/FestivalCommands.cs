using LoanDefinition.Application.DTOs;
using MediatR;

namespace LoanDefinition.Application.Features.Festivals.Commands;

public record CreateFestivalCommand(long FestivalId, string Description, DateTime StartDate, DateTime EndDate, long ModifiedBy)
    : IRequest<LoanFestivalDto>;

public record UpdateFestivalCommand(long FestivalId, string Description, DateTime StartDate, DateTime EndDate, long ModifiedBy)
    : IRequest<LoanFestivalDto>;

public record DeleteFestivalCommand(long FestivalId) : IRequest<bool>;
