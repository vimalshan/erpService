using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PromotionService.Domain.Entities;
using PromotionService.Domain.Events;
using PromotionService.DTOs;
using PromotionService.Features.Commands;
using PromotionService.Infrastructure.Repositories;

namespace PromotionService.Features.Handlers;

#region Rating CommandHandlers
public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, RatingDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateRatingCommandHandler> _logger;

    public CreateRatingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateRatingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<RatingDto> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating rating for employee: {request.EmployeeSystemId}");
        
        // Calculate final rating using weighted average: Appraisal(40%) + Competency(30%) + Goal(30%)
        var finalRating = (request.AppraisalScore * 0.40m) + (request.CompetencyScore * 0.30m) + (request.GoalCompletionScore * 0.30m);
        
        // Determine grade based on final rating
        string grade = finalRating >= 4.5m ? "A" : finalRating >= 3.5m ? "B" : finalRating >= 2.5m ? "C" : "D";
        string category = finalRating >= 4.5m ? "Exceptional" : finalRating >= 2.5m ? "Normal" : "Below";

        var rating = new Rating
        {
            EmployeeSystemId = request.EmployeeSystemId,
            DDYear = request.DDYear,
            AppraisalScore = request.AppraisalScore,
            CompetencyScore = request.CompetencyScore,
            GoalCompletionScore = request.GoalCompletionScore,
            FinalRating = finalRating,
            RatingGrade = grade,
            RatingCategory = category,
            Status = "P",
            RatedOn = DateTime.UtcNow,
            CreatedOn = DateTime.UtcNow
        };

        rating.AddDomainEvent(new RatingCreatedEvent(
                rating.RatingId, rating.EmployeeSystemId, rating.DDYear,
                rating.FinalRating, rating.RatingGrade, DateTime.UtcNow));

        _unitOfWork.Ratings.Add(rating);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Rating created: {rating.RatingId}");
        return _mapper.Map<RatingDto>(rating);
    }
}

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateRatingCommandHandler> _logger;

    public UpdateRatingCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateRatingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating rating: {request.RatingId}");
        var rating = await _unitOfWork.Ratings.GetByIdAsync(request.RatingId, cancellationToken);
        if (rating == null) throw new KeyNotFoundException();

        rating.AppraisalScore = request.AppraisalScore;
        rating.CompetencyScore = request.CompetencyScore;
        rating.GoalCompletionScore = request.GoalCompletionScore;
        
        // Recalculate final rating
        rating.FinalRating = (request.AppraisalScore * 0.40m) + (request.CompetencyScore * 0.30m) + (request.GoalCompletionScore * 0.30m);
        rating.RatingGrade = rating.FinalRating >= 4.5m ? "A" : rating.FinalRating >= 3.5m ? "B" : rating.FinalRating >= 2.5m ? "C" : "D";
        rating.RatingCategory = rating.FinalRating >= 4.5m ? "Exceptional" : rating.FinalRating >= 2.5m ? "Normal" : "Below";

        _unitOfWork.Ratings.Update(rating);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class FinalizeRatingCommandHandler : IRequestHandler<FinalizeRatingCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FinalizeRatingCommandHandler> _logger;

    public FinalizeRatingCommandHandler(IUnitOfWork unitOfWork, ILogger<FinalizeRatingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(FinalizeRatingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Finalizing rating: {request.RatingId}");
        var rating = await _unitOfWork.Ratings.GetByIdAsync(request.RatingId, cancellationToken);
        if (rating == null) throw new KeyNotFoundException();

        rating.Status = "F";
        _unitOfWork.Ratings.Update(rating);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteRatingCommandHandler> _logger;

    public DeleteRatingCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteRatingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Deleting rating: {request.RatingId}");
        var rating = await _unitOfWork.Ratings.GetByIdAsync(request.RatingId, cancellationToken);
        if (rating == null) throw new KeyNotFoundException();

        _unitOfWork.Ratings.Delete(rating);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
#endregion

#region PromotionRecommendation CommandHandlers
public class CreatePromotionRecommendationCommandHandler : IRequestHandler<CreatePromotionRecommendationCommand, PromotionRecommendationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePromotionRecommendationCommandHandler> _logger;

    public CreatePromotionRecommendationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreatePromotionRecommendationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PromotionRecommendationDto> Handle(CreatePromotionRecommendationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating promotion recommendation for employee: {request.EmployeeSystemId}");
        var promotion = new PromotionRecommendation
        {
            RatingId = request.RatingId,
            EmployeeSystemId = request.EmployeeSystemId,
            CurrentDesignation = request.CurrentDesignation,
            CurrentGrade = request.CurrentGrade,
            ProposedDesignation = request.ProposedDesignation,
            ProposedGrade = request.ProposedGrade,
            PromotionEffectiveDate = request.PromotionEffectiveDate,
            ProposedSalaryIncrease = request.ProposedSalaryIncrease,
            PromotionReason = request.PromotionReason,
            Status = "P",
            CreatedOn = DateTime.UtcNow
        };

        _unitOfWork.PromotionRecommendations.Add(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Promotion recommendation created: {promotion.PromotionId}");
        return _mapper.Map<PromotionRecommendationDto>(promotion);
    }
}

public class UpdatePromotionRecommendationCommandHandler : IRequestHandler<UpdatePromotionRecommendationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePromotionRecommendationCommandHandler> _logger;

    public UpdatePromotionRecommendationCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdatePromotionRecommendationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdatePromotionRecommendationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating promotion: {request.PromotionId}");
        var promotion = await _unitOfWork.PromotionRecommendations.GetByIdAsync(request.PromotionId, cancellationToken);
        if (promotion == null) throw new KeyNotFoundException();

        promotion.ProposedDesignation = request.ProposedDesignation;
        promotion.ProposedGrade = request.ProposedGrade;
        promotion.PromotionEffectiveDate = request.PromotionEffectiveDate;
        promotion.ProposedSalaryIncrease = request.ProposedSalaryIncrease;
        promotion.PromotionReason = request.PromotionReason;

        _unitOfWork.PromotionRecommendations.Update(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class ApprovePromotionRecommendationCommandHandler : IRequestHandler<ApprovePromotionRecommendationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApprovePromotionRecommendationCommandHandler> _logger;

    public ApprovePromotionRecommendationCommandHandler(IUnitOfWork unitOfWork, ILogger<ApprovePromotionRecommendationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(ApprovePromotionRecommendationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Approving promotion: {request.PromotionId}");
        var promotion = await _unitOfWork.PromotionRecommendations.GetByIdAsync(request.PromotionId, cancellationToken);
        if (promotion == null) throw new KeyNotFoundException();

        promotion.Status = "A";
        promotion.ApprovedOn = DateTime.UtcNow;
        promotion.ApprovedBySystemId = long.Parse(request.ApprovedBySystemId);
        promotion.UpdatedOn = DateTime.UtcNow;

        promotion.AddDomainEvent(new PromotionApprovedEvent(
            promotion.PromotionId, promotion.EmployeeSystemId,
            promotion.ProposedGrade, promotion.ProposedSalaryIncrease,
            request.ApprovedBySystemId, DateTime.UtcNow));

        _unitOfWork.PromotionRecommendations.Update(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class RejectPromotionRecommendationCommandHandler : IRequestHandler<RejectPromotionRecommendationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectPromotionRecommendationCommandHandler> _logger;

    public RejectPromotionRecommendationCommandHandler(IUnitOfWork unitOfWork, ILogger<RejectPromotionRecommendationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(RejectPromotionRecommendationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Rejecting promotion: {request.PromotionId}");
        var promotion = await _unitOfWork.PromotionRecommendations.GetByIdAsync(request.PromotionId, cancellationToken);
        if (promotion == null) throw new KeyNotFoundException();

        promotion.Status = "R";
        _unitOfWork.PromotionRecommendations.Update(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class HoldPromotionRecommendationCommandHandler : IRequestHandler<HoldPromotionRecommendationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HoldPromotionRecommendationCommandHandler> _logger;

    public HoldPromotionRecommendationCommandHandler(IUnitOfWork unitOfWork, ILogger<HoldPromotionRecommendationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(HoldPromotionRecommendationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Holding promotion: {request.PromotionId}");
        var promotion = await _unitOfWork.PromotionRecommendations.GetByIdAsync(request.PromotionId, cancellationToken);
        if (promotion == null) throw new KeyNotFoundException();

        promotion.Status = "H";
        _unitOfWork.PromotionRecommendations.Update(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeletePromotionRecommendationCommandHandler : IRequestHandler<DeletePromotionRecommendationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePromotionRecommendationCommandHandler> _logger;

    public DeletePromotionRecommendationCommandHandler(IUnitOfWork unitOfWork, ILogger<DeletePromotionRecommendationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeletePromotionRecommendationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Deleting promotion: {request.PromotionId}");
        var promotion = await _unitOfWork.PromotionRecommendations.GetByIdAsync(request.PromotionId, cancellationToken);
        if (promotion == null) throw new KeyNotFoundException();

        _unitOfWork.PromotionRecommendations.Delete(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
#endregion

#region IncrementRequest CommandHandlers
public class CreateIncrementRequestCommandHandler : IRequestHandler<CreateIncrementRequestCommand, IncrementRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateIncrementRequestCommandHandler> _logger;

    public CreateIncrementRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateIncrementRequestCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IncrementRequestDto> Handle(CreateIncrementRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating increment request for employee: {request.EmployeeSystemId}");
        
        var incrementAmount = request.ProposedBaseSalary - request.CurrentBaseSalary;
        var incrementPercentage = (incrementAmount / request.CurrentBaseSalary) * 100;

        var increment = new IncrementRequest
        {
            RatingId = request.RatingId,
            EmployeeSystemId = request.EmployeeSystemId,
            IncrementType = request.IncrementType,
            CurrentBaseSalary = request.CurrentBaseSalary,
            ProposedBaseSalary = request.ProposedBaseSalary,
            IncrementAmount = incrementAmount,
            IncrementPercentage = incrementPercentage,
            IncrementReason = request.IncrementReason,
            EffectiveFromDate = request.EffectiveFromDate,
            Status = "P",
            CreatedOn = DateTime.UtcNow
        };

        _unitOfWork.IncrementRequests.Add(increment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Increment request created: {increment.IncrementId}");
        return _mapper.Map<IncrementRequestDto>(increment);
    }
}

public class UpdateIncrementRequestCommandHandler : IRequestHandler<UpdateIncrementRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateIncrementRequestCommandHandler> _logger;

    public UpdateIncrementRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateIncrementRequestCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateIncrementRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating increment: {request.IncrementId}");
        var increment = await _unitOfWork.IncrementRequests.GetByIdAsync(request.IncrementId, cancellationToken);
        if (increment == null) throw new KeyNotFoundException();

        increment.ProposedBaseSalary = request.ProposedBaseSalary;
        increment.IncrementAmount = request.ProposedBaseSalary - increment.CurrentBaseSalary;
        increment.IncrementPercentage = (increment.IncrementAmount / increment.CurrentBaseSalary) * 100;
        increment.IncrementReason = request.IncrementReason;
        increment.EffectiveFromDate = request.EffectiveFromDate;

        _unitOfWork.IncrementRequests.Update(increment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class ApproveIncrementRequestCommandHandler : IRequestHandler<ApproveIncrementRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveIncrementRequestCommandHandler> _logger;

    public ApproveIncrementRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<ApproveIncrementRequestCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(ApproveIncrementRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Approving increment: {request.IncrementId}");
        var increment = await _unitOfWork.IncrementRequests.GetByIdAsync(request.IncrementId, cancellationToken);
        if (increment == null) throw new KeyNotFoundException();

        increment.Status = "A";
        increment.ApprovedOn = DateTime.UtcNow;

        _unitOfWork.IncrementRequests.Update(increment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class RejectIncrementRequestCommandHandler : IRequestHandler<RejectIncrementRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectIncrementRequestCommandHandler> _logger;

    public RejectIncrementRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<RejectIncrementRequestCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(RejectIncrementRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Rejecting increment: {request.IncrementId}");
        var increment = await _unitOfWork.IncrementRequests.GetByIdAsync(request.IncrementId, cancellationToken);
        if (increment == null) throw new KeyNotFoundException();

        increment.Status = "R";
        increment.RejectionReason = request.ReasonForRejection;
        increment.UpdatedOn = DateTime.UtcNow;
        increment.AddDomainEvent(new IncrementRejectedEvent(
            increment.IncrementId, increment.EmployeeSystemId,
            request.ReasonForRejection, DateTime.UtcNow));
        _unitOfWork.IncrementRequests.Update(increment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteIncrementRequestCommandHandler : IRequestHandler<DeleteIncrementRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteIncrementRequestCommandHandler> _logger;

    public DeleteIncrementRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteIncrementRequestCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteIncrementRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Deleting increment: {request.IncrementId}");
        var increment = await _unitOfWork.IncrementRequests.GetByIdAsync(request.IncrementId, cancellationToken);
        if (increment == null) throw new KeyNotFoundException();

        _unitOfWork.IncrementRequests.Delete(increment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
#endregion

#region VTCAssessment CommandHandlers
public class CreateVTCAssessmentCommandHandler : IRequestHandler<CreateVTCAssessmentCommand, VTCAssessmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateVTCAssessmentCommandHandler> _logger;

    public CreateVTCAssessmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateVTCAssessmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<VTCAssessmentDto> Handle(CreateVTCAssessmentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating VTC assessment for employee: {request.EmployeeSystemId}");
        var assessment = new VTCAssessment
        {
            EmployeeSystemId = request.EmployeeSystemId,
            DDYear = request.DDYear,
            Quarter = request.Quarter,
            Score = request.Score,
            AssessedOn = DateTime.UtcNow,
            CreatedOn = DateTime.UtcNow
        };

        _unitOfWork.VTCAssessments.Add(assessment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"VTC assessment created: {assessment.VTCAssessmentId}");
        return _mapper.Map<VTCAssessmentDto>(assessment);
    }
}

public class UpdateVTCAssessmentCommandHandler : IRequestHandler<UpdateVTCAssessmentCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateVTCAssessmentCommandHandler> _logger;

    public UpdateVTCAssessmentCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateVTCAssessmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateVTCAssessmentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating VTC assessment: {request.VTCAssessmentId}");
        var assessment = await _unitOfWork.VTCAssessments.GetByIdAsync(request.VTCAssessmentId, cancellationToken);
        if (assessment == null) throw new KeyNotFoundException();

        assessment.Score = request.Score;
        _unitOfWork.VTCAssessments.Update(assessment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteVTCAssessmentCommandHandler : IRequestHandler<DeleteVTCAssessmentCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteVTCAssessmentCommandHandler> _logger;

    public DeleteVTCAssessmentCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteVTCAssessmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteVTCAssessmentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Deleting VTC assessment: {request.VTCAssessmentId}");
        var assessment = await _unitOfWork.VTCAssessments.GetByIdAsync(request.VTCAssessmentId, cancellationToken);
        if (assessment == null) throw new KeyNotFoundException();

        _unitOfWork.VTCAssessments.Delete(assessment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
#endregion

#region HorizontalPromotion CommandHandlers
public class CreateHorizontalPromotionCommandHandler : IRequestHandler<CreateHorizontalPromotionCommand, HorizontalPromotionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateHorizontalPromotionCommandHandler> _logger;

    public CreateHorizontalPromotionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateHorizontalPromotionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<HorizontalPromotionDto> Handle(CreateHorizontalPromotionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating horizontal promotion for employee: {request.EmployeeSystemId}");
        var hp = new HorizontalPromotion
        {
            TransactionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            EmployeeSystemId = request.EmployeeSystemId,
            PromotionScore = request.PromotionScore,
            GradeId = request.GradeId,
            CurrentLevelId = request.CurrentLevelId,
            NewLevelId = request.NewLevelId,
            EffectiveFrom = request.EffectiveFrom,
            PositionId = request.PositionId,
            OldPositionName = request.OldPositionName,
            OldPositionDesignation = request.OldPositionDesignation,
            NewPositionName = request.NewPositionName,
            NewPositionDesignation = request.NewPositionDesignation,
            UpdatedBy = request.UpdatedBy,
            UpdatedOn = DateTime.UtcNow,
            ConfirmHrms = "N"
        };
        _unitOfWork.HorizontalPromotions.Add(hp);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<HorizontalPromotionDto>(hp);
    }
}

public class ConfirmHorizontalPromotionCommandHandler : IRequestHandler<ConfirmHorizontalPromotionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ConfirmHorizontalPromotionCommandHandler> _logger;

    public ConfirmHorizontalPromotionCommandHandler(IUnitOfWork unitOfWork, ILogger<ConfirmHorizontalPromotionCommandHandler> logger)
    { _unitOfWork = unitOfWork; _logger = logger; }

    public async Task<bool> Handle(ConfirmHorizontalPromotionCommand request, CancellationToken cancellationToken)
    {
        var hp = await _unitOfWork.HorizontalPromotions.GetByIdAsync(request.TransactionId, cancellationToken);
        if (hp == null) throw new KeyNotFoundException();
        hp.ConfirmHrms = "Y";
        hp.PosUpdatedOn = DateTime.UtcNow;
        _unitOfWork.HorizontalPromotions.Update(hp);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteHorizontalPromotionCommandHandler : IRequestHandler<DeleteHorizontalPromotionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteHorizontalPromotionCommandHandler> _logger;

    public DeleteHorizontalPromotionCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteHorizontalPromotionCommandHandler> logger)
    { _unitOfWork = unitOfWork; _logger = logger; }

    public async Task<bool> Handle(DeleteHorizontalPromotionCommand request, CancellationToken cancellationToken)
    {
        var hp = await _unitOfWork.HorizontalPromotions.GetByIdAsync(request.TransactionId, cancellationToken);
        if (hp == null) throw new KeyNotFoundException();
        _unitOfWork.HorizontalPromotions.Delete(hp);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
#endregion

#region VTCCorrection CommandHandlers
public class CreateVTCCorrectionCommandHandler : IRequestHandler<CreateVTCCorrectionCommand, VTCCorrectionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateVTCCorrectionCommandHandler> _logger;

    public CreateVTCCorrectionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateVTCCorrectionCommandHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<VTCCorrectionDto> Handle(CreateVTCCorrectionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating VTC correction for employee: {request.EmployeeSystemId}");
        var correction = new VTCCorrection
        {
            RateId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            EmployeeSystemId = request.EmployeeSystemId,
            FinancialYearId = request.FinancialYearId,
            GradeId = request.GradeId,
            Status = "P",
            OldRating = request.OldRating,
            NewRating = request.NewRating,
            OldCash = request.OldCash,
            NewCash = request.NewCash,
            OldPromotion = request.OldPromotion,
            NewPromotion = request.NewPromotion,
            OldRationalization = request.OldRationalization,
            NewRationalization = request.NewRationalization,
            Reason = request.Reason,
            CreatedBy = request.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };
        _unitOfWork.VTCCorrections.Add(correction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<VTCCorrectionDto>(correction);
    }
}

public class ApproveVTCCorrectionCommandHandler : IRequestHandler<ApproveVTCCorrectionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveVTCCorrectionCommandHandler> _logger;

    public ApproveVTCCorrectionCommandHandler(IUnitOfWork unitOfWork, ILogger<ApproveVTCCorrectionCommandHandler> logger)
    { _unitOfWork = unitOfWork; _logger = logger; }

    public async Task<bool> Handle(ApproveVTCCorrectionCommand request, CancellationToken cancellationToken)
    {
        var correction = await _unitOfWork.VTCCorrections.GetByIdAsync(request.RateId, cancellationToken);
        if (correction == null) throw new KeyNotFoundException();
        correction.Status = "A";
        correction.ApprovedBy = request.ApprovedBy;
        correction.ApprovedOn = DateTime.UtcNow;
        _unitOfWork.VTCCorrections.Update(correction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class RejectVTCCorrectionCommandHandler : IRequestHandler<RejectVTCCorrectionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectVTCCorrectionCommandHandler> _logger;

    public RejectVTCCorrectionCommandHandler(IUnitOfWork unitOfWork, ILogger<RejectVTCCorrectionCommandHandler> logger)
    { _unitOfWork = unitOfWork; _logger = logger; }

    public async Task<bool> Handle(RejectVTCCorrectionCommand request, CancellationToken cancellationToken)
    {
        var correction = await _unitOfWork.VTCCorrections.GetByIdAsync(request.RateId, cancellationToken);
        if (correction == null) throw new KeyNotFoundException();
        correction.Status = "R";
        correction.ModifiedOn = DateTime.UtcNow;
        _unitOfWork.VTCCorrections.Update(correction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
#endregion

#region DirectIncrement CommandHandlers
public class CreateDirectIncrementCommandHandler : IRequestHandler<CreateDirectIncrementCommand, DirectIncrementDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateDirectIncrementCommandHandler> _logger;

    public CreateDirectIncrementCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateDirectIncrementCommandHandler> logger)
    { _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger; }

    public async Task<DirectIncrementDto> Handle(CreateDirectIncrementCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating direct increment for employee: {request.EmployeeSystemId}");
        var di = new DirectIncrement
        {
            IncrementId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            EmployeeSystemId = request.EmployeeSystemId,
            YearId = request.YearId,
            Amount = request.Amount,
            SalaryType = request.SalaryType,
            UpdatedBy = request.UpdatedBy,
            UpdatedOn = DateTime.UtcNow,
            RatingAmount = request.RatingAmount,
            PromotionAmount = request.PromotionAmount,
            Percent = request.Percent
        };
        _unitOfWork.DirectIncrements.Add(di);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<DirectIncrementDto>(di);
    }
}

public class DeleteDirectIncrementCommandHandler : IRequestHandler<DeleteDirectIncrementCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteDirectIncrementCommandHandler> _logger;

    public DeleteDirectIncrementCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteDirectIncrementCommandHandler> logger)
    { _unitOfWork = unitOfWork; _logger = logger; }

    public async Task<bool> Handle(DeleteDirectIncrementCommand request, CancellationToken cancellationToken)
    {
        var di = await _unitOfWork.DirectIncrements.GetByIdAsync(request.IncrementId, cancellationToken);
        if (di == null) throw new KeyNotFoundException();
        _unitOfWork.DirectIncrements.Delete(di);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
#endregion
