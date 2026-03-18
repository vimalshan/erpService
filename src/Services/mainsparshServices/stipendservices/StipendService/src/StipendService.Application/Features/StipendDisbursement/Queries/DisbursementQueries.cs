using MediatR;
using StipendService.Application.DTOs;

namespace StipendService.Application.Features.StipendDisbursement.Queries;

public record GetDisbursementByIdQuery(long DisbursementId) : IRequest<StipendDisbursementDto?>;
public record GetDisbursementsByMonthQuery(string MonthYear) : IRequest<IEnumerable<StipendDisbursementDto>>;
public record GetDisbursementsBySrfQuery(long SrfId) : IRequest<IEnumerable<StipendDisbursementDto>>;
