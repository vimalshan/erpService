using AuditService.Application.DTOs;
using MediatR;

namespace AuditService.Application.Queries.GoodPractices;

public record GetGoodPracticeByIdQuery(long PracticeId) : IRequest<GoodPracticeDto?>;
public record GetAllGoodPracticesQuery : IRequest<IEnumerable<GoodPracticeDto>>;
public record GetGoodPracticesByUnitQuery(long UnitId) : IRequest<IEnumerable<GoodPracticeDto>>;
