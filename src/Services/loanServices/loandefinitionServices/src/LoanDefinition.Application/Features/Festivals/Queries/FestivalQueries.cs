using LoanDefinition.Application.DTOs;
using MediatR;

namespace LoanDefinition.Application.Features.Festivals.Queries;

public record GetAllFestivalsQuery : IRequest<IReadOnlyList<LoanFestivalDto>>;
public record GetFestivalByIdQuery(long FestivalId) : IRequest<LoanFestivalDto?>;
public record GetActiveFestivalsQuery(DateTime AsOfDate) : IRequest<IReadOnlyList<LoanFestivalDto>>;
