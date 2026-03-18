using AuditService.Application.DTOs;
using AuditService.Domain.Entities;
using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Commands.GoodPractices;

public sealed class CreateGoodPracticeCommandHandler : IRequestHandler<CreateGoodPracticeCommand, GoodPracticeDto>
{
    private readonly IGoodPracticeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGoodPracticeCommandHandler(IGoodPracticeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GoodPracticeDto> Handle(CreateGoodPracticeCommand request, CancellationToken cancellationToken)
    {
        var practice = AuditGoodPractice.Create(
            request.PracticeId, request.PracticeTitle, request.PracticeDescription,
            request.PracticeBenefits, request.PracticeRemarks, request.PracticeProcess,
            request.PracticeEmpSysId, request.PracticeUnit, request.CreatedBy);

        await _repository.AddAsync(practice, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(practice);
    }

    private static GoodPracticeDto ToDto(AuditGoodPractice p) => new(
        p.PracticeId, p.PracticeTitle, p.PracticeDescription, p.PracticeBenefits,
        p.PracticeRemarks, p.PracticeProcess, p.PracticeEmpSysId, p.PracticeUnit,
        p.PracticeLastModifiedOn, p.AverageRating, p.Ratings.Count,
        p.PracticeAttachment1, p.PracticeAttachment2);
}

public sealed class RateGoodPracticeCommandHandler : IRequestHandler<RateGoodPracticeCommand, bool>
{
    private readonly IGoodPracticeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RateGoodPracticeCommandHandler(IGoodPracticeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RateGoodPracticeCommand request, CancellationToken cancellationToken)
    {
        var practice = await _repository.GetByIdAsync(request.PracticeId, cancellationToken);
        if (practice is null) return false;

        practice.AddRating(request.RatingId, request.RatedBy, request.Rating);
        await _repository.UpdateAsync(practice, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
