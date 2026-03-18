using AuditService.Application.DTOs;
using AuditService.Domain.Entities;
using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Queries.GoodPractices;

public sealed class GetGoodPracticeByIdQueryHandler : IRequestHandler<GetGoodPracticeByIdQuery, GoodPracticeDto?>
{
    private readonly IGoodPracticeRepository _repository;

    public GetGoodPracticeByIdQueryHandler(IGoodPracticeRepository repository) => _repository = repository;

    public async Task<GoodPracticeDto?> Handle(GetGoodPracticeByIdQuery request, CancellationToken cancellationToken)
    {
        var p = await _repository.GetByIdAsync(request.PracticeId, cancellationToken);
        return p is null ? null : ToDto(p);
    }

    private static GoodPracticeDto ToDto(AuditGoodPractice p) => new(
        p.PracticeId, p.PracticeTitle, p.PracticeDescription, p.PracticeBenefits,
        p.PracticeRemarks, p.PracticeProcess, p.PracticeEmpSysId, p.PracticeUnit,
        p.PracticeLastModifiedOn, p.AverageRating, p.Ratings.Count,
        p.PracticeAttachment1, p.PracticeAttachment2);
}

public sealed class GetAllGoodPracticesQueryHandler : IRequestHandler<GetAllGoodPracticesQuery, IEnumerable<GoodPracticeDto>>
{
    private readonly IGoodPracticeRepository _repository;

    public GetAllGoodPracticesQueryHandler(IGoodPracticeRepository repository) => _repository = repository;

    public async Task<IEnumerable<GoodPracticeDto>> Handle(GetAllGoodPracticesQuery request, CancellationToken cancellationToken)
    {
        var practices = await _repository.GetAllAsync(cancellationToken);
        return practices.Select(p => new GoodPracticeDto(
            p.PracticeId, p.PracticeTitle, p.PracticeDescription, p.PracticeBenefits,
            p.PracticeRemarks, p.PracticeProcess, p.PracticeEmpSysId, p.PracticeUnit,
            p.PracticeLastModifiedOn, p.AverageRating, p.Ratings.Count,
            p.PracticeAttachment1, p.PracticeAttachment2));
    }
}
