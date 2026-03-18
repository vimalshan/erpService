using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PromotionService.Domain.Entities;
using PromotionService.DTOs;
using PromotionService.Features.Queries;
using PromotionService.Infrastructure.Repositories;

namespace PromotionService.Features.Handlers;

#region Rating QueryHandlers
public class GetRatingByIdQueryHandler : IRequestHandler<GetRatingByIdQuery, RatingDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRatingByIdQueryHandler> _logger;

    public GetRatingByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetRatingByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<RatingDto> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching rating: {request.RatingId}");
        var rating = await _unitOfWork.Ratings.GetByIdAsync(request.RatingId, cancellationToken);
        if (rating == null) throw new KeyNotFoundException();
        return _mapper.Map<RatingDto>(rating);
    }
}

public class GetRatingsByEmployeeQueryHandler : IRequestHandler<GetRatingsByEmployeeQuery, IEnumerable<RatingDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRatingsByEmployeeQueryHandler> _logger;

    public GetRatingsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetRatingsByEmployeeQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<RatingDto>> Handle(GetRatingsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching ratings for employee: {request.EmployeeSystemId}");
        var ratings = await _unitOfWork.Ratings.AsQueryable()
            .Where(r => r.EmployeeSystemId == request.EmployeeSystemId)
            .OrderByDescending(r => r.DDYear)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<RatingDto>>(ratings);
    }
}

public class GetAllRatingsQueryHandler : IRequestHandler<GetAllRatingsQuery, IEnumerable<RatingDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllRatingsQueryHandler> _logger;

    public GetAllRatingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllRatingsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<RatingDto>> Handle(GetAllRatingsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching all ratings - Page: {request.PageNumber}");
        var query = _unitOfWork.Ratings.AsQueryable();

        if (request.DDYear.HasValue)
            query = query.Where(r => r.DDYear == request.DDYear);
        if (!string.IsNullOrEmpty(request.Status))
            query = query.Where(r => r.Status == request.Status);

        var ratings = await query
            .OrderByDescending(r => r.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<RatingDto>>(ratings);
    }
}

public class GetPendingRatingsQueryHandler : IRequestHandler<GetPendingRatingsQuery, IEnumerable<RatingDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPendingRatingsQueryHandler> _logger;

    public GetPendingRatingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPendingRatingsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<RatingDto>> Handle(GetPendingRatingsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching pending ratings - Page: {request.PageNumber}");
        var ratings = await _unitOfWork.Ratings.AsQueryable()
            .Where(r => r.Status == "P")
            .OrderBy(r => r.RatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<RatingDto>>(ratings);
    }
}

public class GetHighPerformerRatingsQueryHandler : IRequestHandler<GetHighPerformerRatingsQuery, IEnumerable<RatingDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetHighPerformerRatingsQueryHandler> _logger;

    public GetHighPerformerRatingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetHighPerformerRatingsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<RatingDto>> Handle(GetHighPerformerRatingsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching high performer ratings for year: {request.DDYear}");
        var ratings = await _unitOfWork.Ratings.AsQueryable()
            .Where(r => r.DDYear == request.DDYear && r.RatingGrade == "A")
            .OrderByDescending(r => r.FinalRating)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<RatingDto>>(ratings);
    }
}
#endregion

#region PromotionRecommendation QueryHandlers
public class GetPromotionByIdQueryHandler : IRequestHandler<GetPromotionByIdQuery, PromotionRecommendationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPromotionByIdQueryHandler> _logger;

    public GetPromotionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPromotionByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PromotionRecommendationDto> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching promotion: {request.PromotionId}");
        var promotion = await _unitOfWork.PromotionRecommendations.GetByIdAsync(request.PromotionId, cancellationToken);
        if (promotion == null) throw new KeyNotFoundException();
        return _mapper.Map<PromotionRecommendationDto>(promotion);
    }
}

public class GetPromotionsByEmployeeQueryHandler : IRequestHandler<GetPromotionsByEmployeeQuery, IEnumerable<PromotionRecommendationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPromotionsByEmployeeQueryHandler> _logger;

    public GetPromotionsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPromotionsByEmployeeQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PromotionRecommendationDto>> Handle(GetPromotionsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching promotions for employee: {request.EmployeeSystemId}");
        var promotions = await _unitOfWork.PromotionRecommendations.AsQueryable()
            .Where(p => p.EmployeeSystemId == request.EmployeeSystemId)
            .OrderByDescending(p => p.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PromotionRecommendationDto>>(promotions);
    }
}

public class GetAllPromotionsQueryHandler : IRequestHandler<GetAllPromotionsQuery, IEnumerable<PromotionRecommendationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllPromotionsQueryHandler> _logger;

    public GetAllPromotionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllPromotionsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PromotionRecommendationDto>> Handle(GetAllPromotionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching all promotions - Page: {request.PageNumber}");
        var query = _unitOfWork.PromotionRecommendations.AsQueryable();

        if (!string.IsNullOrEmpty(request.Status))
            query = query.Where(p => p.Status == request.Status);

        var promotions = await query
            .OrderByDescending(p => p.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PromotionRecommendationDto>>(promotions);
    }
}

public class GetPendingPromotionsQueryHandler : IRequestHandler<GetPendingPromotionsQuery, IEnumerable<PromotionRecommendationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPendingPromotionsQueryHandler> _logger;

    public GetPendingPromotionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPendingPromotionsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PromotionRecommendationDto>> Handle(GetPendingPromotionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching pending promotions - Page: {request.PageNumber}");
        var promotions = await _unitOfWork.PromotionRecommendations.AsQueryable()
            .Where(p => p.Status == "P")
            .OrderBy(p => p.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PromotionRecommendationDto>>(promotions);
    }
}

public class GetPromotionsByRatingQueryHandler : IRequestHandler<GetPromotionsByRatingQuery, IEnumerable<PromotionRecommendationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPromotionsByRatingQueryHandler> _logger;

    public GetPromotionsByRatingQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPromotionsByRatingQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PromotionRecommendationDto>> Handle(GetPromotionsByRatingQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching promotions for rating: {request.RatingId}");
        var promotions = await _unitOfWork.PromotionRecommendations.AsQueryable()
            .Where(p => p.RatingId == request.RatingId)
            .OrderByDescending(p => p.CreatedOn)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PromotionRecommendationDto>>(promotions);
    }
}
#endregion

#region IncrementRequest QueryHandlers
public class GetIncrementByIdQueryHandler : IRequestHandler<GetIncrementByIdQuery, IncrementRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetIncrementByIdQueryHandler> _logger;

    public GetIncrementByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetIncrementByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IncrementRequestDto> Handle(GetIncrementByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching increment: {request.IncrementId}");
        var increment = await _unitOfWork.IncrementRequests.GetByIdAsync(request.IncrementId, cancellationToken);
        if (increment == null) throw new KeyNotFoundException();
        return _mapper.Map<IncrementRequestDto>(increment);
    }
}

public class GetIncrementsByEmployeeQueryHandler : IRequestHandler<GetIncrementsByEmployeeQuery, IEnumerable<IncrementRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetIncrementsByEmployeeQueryHandler> _logger;

    public GetIncrementsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetIncrementsByEmployeeQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<IncrementRequestDto>> Handle(GetIncrementsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching increments for employee: {request.EmployeeSystemId}");
        var increments = await _unitOfWork.IncrementRequests.AsQueryable()
            .Where(i => i.EmployeeSystemId == request.EmployeeSystemId)
            .OrderByDescending(i => i.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<IncrementRequestDto>>(increments);
    }
}

public class GetAllIncrementsQueryHandler : IRequestHandler<GetAllIncrementsQuery, IEnumerable<IncrementRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllIncrementsQueryHandler> _logger;

    public GetAllIncrementsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllIncrementsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<IncrementRequestDto>> Handle(GetAllIncrementsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching all increments - Page: {request.PageNumber}");
        var query = _unitOfWork.IncrementRequests.AsQueryable();

        if (!string.IsNullOrEmpty(request.IncrementType))
            query = query.Where(i => i.IncrementType == request.IncrementType);
        if (!string.IsNullOrEmpty(request.Status))
            query = query.Where(i => i.Status == request.Status);

        var increments = await query
            .OrderByDescending(i => i.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<IncrementRequestDto>>(increments);
    }
}

public class GetPendingIncrementsQueryHandler : IRequestHandler<GetPendingIncrementsQuery, IEnumerable<IncrementRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPendingIncrementsQueryHandler> _logger;

    public GetPendingIncrementsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPendingIncrementsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<IncrementRequestDto>> Handle(GetPendingIncrementsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching pending increments - Page: {request.PageNumber}");
        var increments = await _unitOfWork.IncrementRequests.AsQueryable()
            .Where(i => i.Status == "P")
            .OrderBy(i => i.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<IncrementRequestDto>>(increments);
    }
}

public class GetIncrementsByRatingQueryHandler : IRequestHandler<GetIncrementsByRatingQuery, IEnumerable<IncrementRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetIncrementsByRatingQueryHandler> _logger;

    public GetIncrementsByRatingQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetIncrementsByRatingQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<IncrementRequestDto>> Handle(GetIncrementsByRatingQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching increments for rating: {request.RatingId}");
        var increments = await _unitOfWork.IncrementRequests.AsQueryable()
            .Where(i => i.RatingId == request.RatingId)
            .OrderByDescending(i => i.CreatedOn)
            .ToListAsync();
        return _mapper.Map<IEnumerable<IncrementRequestDto>>(increments);
    }
}
#endregion

#region VTCAssessment QueryHandlers
public class GetVTCAssessmentByIdQueryHandler : IRequestHandler<GetVTCAssessmentByIdQuery, VTCAssessmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetVTCAssessmentByIdQueryHandler> _logger;

    public GetVTCAssessmentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetVTCAssessmentByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<VTCAssessmentDto> Handle(GetVTCAssessmentByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching VTC assessment: {request.VTCAssessmentId}");
        var assessment = await _unitOfWork.VTCAssessments.GetByIdAsync(request.VTCAssessmentId, cancellationToken);
        if (assessment == null) throw new KeyNotFoundException();
        return _mapper.Map<VTCAssessmentDto>(assessment);
    }
}

public class GetVTCAssessmentsByEmployeeQueryHandler : IRequestHandler<GetVTCAssessmentsByEmployeeQuery, IEnumerable<VTCAssessmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetVTCAssessmentsByEmployeeQueryHandler> _logger;

    public GetVTCAssessmentsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetVTCAssessmentsByEmployeeQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<VTCAssessmentDto>> Handle(GetVTCAssessmentsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching VTC assessments for employee: {request.EmployeeSystemId}");
        var assessments = await _unitOfWork.VTCAssessments.AsQueryable()
            .Where(v => v.EmployeeSystemId == request.EmployeeSystemId)
            .OrderByDescending(v => v.DDYear)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<VTCAssessmentDto>>(assessments);
    }
}

public class GetVTCAssessmentsByYearQueryHandler : IRequestHandler<GetVTCAssessmentsByYearQuery, IEnumerable<VTCAssessmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetVTCAssessmentsByYearQueryHandler> _logger;

    public GetVTCAssessmentsByYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetVTCAssessmentsByYearQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<VTCAssessmentDto>> Handle(GetVTCAssessmentsByYearQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Fetching VTC assessments for year: {request.DDYear}");
        var assessments = await _unitOfWork.VTCAssessments.AsQueryable()
            .Where(v => v.DDYear == request.DDYear)
            .OrderByDescending(v => v.Score)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return _mapper.Map<IEnumerable<VTCAssessmentDto>>(assessments);
    }
}
#endregion

#region HorizontalPromotion QueryHandlers
public class GetHorizontalPromotionByIdQueryHandler : IRequestHandler<GetHorizontalPromotionByIdQuery, HorizontalPromotionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetHorizontalPromotionByIdQueryHandler> _logger;

    public GetHorizontalPromotionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetHorizontalPromotionByIdQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<HorizontalPromotionDto> Handle(GetHorizontalPromotionByIdQuery request, CancellationToken cancellationToken)
    {
        var hp = await _unitOfWork.HorizontalPromotions.GetByIdAsync(request.TransactionId, cancellationToken);
        if (hp == null) throw new KeyNotFoundException();
        return _mapper.Map<HorizontalPromotionDto>(hp);
    }
}

public class GetHorizontalPromotionsByEmployeeQueryHandler : IRequestHandler<GetHorizontalPromotionsByEmployeeQuery, IEnumerable<HorizontalPromotionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetHorizontalPromotionsByEmployeeQueryHandler> _logger;

    public GetHorizontalPromotionsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetHorizontalPromotionsByEmployeeQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<HorizontalPromotionDto>> Handle(GetHorizontalPromotionsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.HorizontalPromotions.AsQueryable()
            .Where(h => h.EmployeeSystemId == request.EmployeeSystemId)
            .OrderByDescending(h => h.EffectiveFrom)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<HorizontalPromotionDto>>(list);
    }
}

public class GetAllHorizontalPromotionsQueryHandler : IRequestHandler<GetAllHorizontalPromotionsQuery, IEnumerable<HorizontalPromotionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllHorizontalPromotionsQueryHandler> _logger;

    public GetAllHorizontalPromotionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllHorizontalPromotionsQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<HorizontalPromotionDto>> Handle(GetAllHorizontalPromotionsQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.HorizontalPromotions.AsQueryable()
            .OrderByDescending(h => h.EffectiveFrom)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<HorizontalPromotionDto>>(list);
    }
}
#endregion

#region AppraisalAmount QueryHandlers
public class GetAppraisalAmountByIdQueryHandler : IRequestHandler<GetAppraisalAmountByIdQuery, AppraisalAmountDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAppraisalAmountByIdQueryHandler> _logger;

    public GetAppraisalAmountByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAppraisalAmountByIdQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<AppraisalAmountDto> Handle(GetAppraisalAmountByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.AppraisalAmounts.GetByIdAsync(request.SerialNo, cancellationToken);
        if (item == null) throw new KeyNotFoundException();
        return _mapper.Map<AppraisalAmountDto>(item);
    }
}

public class GetAppraisalAmountsByBandQueryHandler : IRequestHandler<GetAppraisalAmountsByBandQuery, IEnumerable<AppraisalAmountDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAppraisalAmountsByBandQueryHandler> _logger;

    public GetAppraisalAmountsByBandQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAppraisalAmountsByBandQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<AppraisalAmountDto>> Handle(GetAppraisalAmountsByBandQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.AppraisalAmounts.FindAsync(a => a.BandId == request.BandId, cancellationToken);
        return _mapper.Map<IEnumerable<AppraisalAmountDto>>(list);
    }
}

public class GetAllAppraisalAmountsQueryHandler : IRequestHandler<GetAllAppraisalAmountsQuery, IEnumerable<AppraisalAmountDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllAppraisalAmountsQueryHandler> _logger;

    public GetAllAppraisalAmountsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllAppraisalAmountsQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<AppraisalAmountDto>> Handle(GetAllAppraisalAmountsQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.AppraisalAmounts.AsQueryable()
            .OrderBy(a => a.BandId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AppraisalAmountDto>>(list);
    }
}
#endregion

#region VTCCorrection QueryHandlers
public class GetVTCCorrectionByIdQueryHandler : IRequestHandler<GetVTCCorrectionByIdQuery, VTCCorrectionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetVTCCorrectionByIdQueryHandler> _logger;

    public GetVTCCorrectionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetVTCCorrectionByIdQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<VTCCorrectionDto> Handle(GetVTCCorrectionByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.VTCCorrections.GetByIdAsync(request.RateId, cancellationToken);
        if (item == null) throw new KeyNotFoundException();
        return _mapper.Map<VTCCorrectionDto>(item);
    }
}

public class GetVTCCorrectionsByEmployeeQueryHandler : IRequestHandler<GetVTCCorrectionsByEmployeeQuery, IEnumerable<VTCCorrectionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetVTCCorrectionsByEmployeeQueryHandler> _logger;

    public GetVTCCorrectionsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetVTCCorrectionsByEmployeeQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<VTCCorrectionDto>> Handle(GetVTCCorrectionsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.VTCCorrections.AsQueryable()
            .Where(v => v.EmployeeSystemId == request.EmployeeSystemId)
            .OrderByDescending(v => v.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<VTCCorrectionDto>>(list);
    }
}

public class GetPendingVTCCorrectionsQueryHandler : IRequestHandler<GetPendingVTCCorrectionsQuery, IEnumerable<VTCCorrectionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPendingVTCCorrectionsQueryHandler> _logger;

    public GetPendingVTCCorrectionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPendingVTCCorrectionsQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<VTCCorrectionDto>> Handle(GetPendingVTCCorrectionsQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.VTCCorrections.AsQueryable()
            .Where(v => v.Status == "P")
            .OrderBy(v => v.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<VTCCorrectionDto>>(list);
    }
}
#endregion

#region DirectIncrement QueryHandlers
public class GetDirectIncrementsByEmployeeQueryHandler : IRequestHandler<GetDirectIncrementsByEmployeeQuery, IEnumerable<DirectIncrementDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDirectIncrementsByEmployeeQueryHandler> _logger;

    public GetDirectIncrementsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetDirectIncrementsByEmployeeQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<DirectIncrementDto>> Handle(GetDirectIncrementsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.DirectIncrements.AsQueryable()
            .Where(d => d.EmployeeSystemId == request.EmployeeSystemId)
            .OrderByDescending(d => d.UpdatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<DirectIncrementDto>>(list);
    }
}

public class GetDirectIncrementsByYearQueryHandler : IRequestHandler<GetDirectIncrementsByYearQuery, IEnumerable<DirectIncrementDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDirectIncrementsByYearQueryHandler> _logger;

    public GetDirectIncrementsByYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetDirectIncrementsByYearQueryHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<DirectIncrementDto>> Handle(GetDirectIncrementsByYearQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.DirectIncrements.AsQueryable()
            .Where(d => d.YearId == request.YearId)
            .OrderByDescending(d => d.UpdatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<DirectIncrementDto>>(list);
    }
}
#endregion
