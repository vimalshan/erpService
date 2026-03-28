using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Queries;

public class GetAllBudgetsQuery : IRequest<IEnumerable<SaaBudgetDto>>
{
    public class Handler : IRequestHandler<GetAllBudgetsQuery, IEnumerable<SaaBudgetDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SaaBudgetDto>> Handle(GetAllBudgetsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.SaaBudgets.GetAllAsync(cancellationToken);
            return entities.Select(e => new SaaBudgetDto
            {
                Id = e.Id, BusinessId = e.BusinessId, YearId = e.YearId,
                BudgetAmount = e.BudgetAmount, UpdatedBy = e.UpdatedBy, UpdatedOn = e.UpdatedOn,
                CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
            });
        }
    }
}

public class GetBudgetsByYearQuery : IRequest<IEnumerable<SaaBudgetDto>>
{
    public long YearId { get; set; }

    public class Handler : IRequestHandler<GetBudgetsByYearQuery, IEnumerable<SaaBudgetDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SaaBudgetDto>> Handle(GetBudgetsByYearQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.SaaBudgets.GetByYearAsync(request.YearId, cancellationToken);
            return entities.Select(e => new SaaBudgetDto
            {
                Id = e.Id, BusinessId = e.BusinessId, YearId = e.YearId,
                BudgetAmount = e.BudgetAmount, UpdatedBy = e.UpdatedBy, UpdatedOn = e.UpdatedOn,
                CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
            });
        }
    }
}

public class GetAllPeriodsQuery : IRequest<IEnumerable<SaaPeriodDto>>
{
    public class Handler : IRequestHandler<GetAllPeriodsQuery, IEnumerable<SaaPeriodDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SaaPeriodDto>> Handle(GetAllPeriodsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.SaaPeriods.GetAllAsync(cancellationToken);
            return entities.Select(e => new SaaPeriodDto
            {
                Id = e.Id, YearId = e.YearId, QuarterNo = e.QuarterNo, Status = e.Status,
                PeriodOpenDate = e.PeriodOpenDate, PeriodCloseDate = e.PeriodCloseDate,
                CircularGenOn = e.CircularGenOn, CircularGenBy = e.CircularGenBy,
                ReminderLetOn = e.ReminderLetOn, FormOpenDate = e.FormOpenDate,
                AppraiserLastDate = e.AppraiserLastDate, ReviewerLastDate = e.ReviewerLastDate,
                BhrLastDate = e.BhrLastDate, UhrLastDate = e.UhrLastDate,
                CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
            });
        }
    }
}

public class GetAllLevelsQuery : IRequest<IEnumerable<SaaLevelDto>>
{
    public class Handler : IRequestHandler<GetAllLevelsQuery, IEnumerable<SaaLevelDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SaaLevelDto>> Handle(GetAllLevelsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.SaaLevels.GetAllAsync(cancellationToken);
            return entities.Select(e => new SaaLevelDto
            {
                Id = e.Id, LevelDesc = e.LevelDesc, LevelAmount = e.LevelAmount,
                LevelReason = e.LevelReason, LevelMin = e.LevelMin, LevelMax = e.LevelMax,
                LevelEffDate = e.LevelEffDate, LevelCloseDate = e.LevelCloseDate,
                LevelUpdatedBy = e.LevelUpdatedBy, LevelUpdatedOn = e.LevelUpdatedOn,
                CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
            });
        }
    }
}

public class GetAllRecommendsQuery : IRequest<IEnumerable<SaaRecommendDto>>
{
    public class Handler : IRequestHandler<GetAllRecommendsQuery, IEnumerable<SaaRecommendDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SaaRecommendDto>> Handle(GetAllRecommendsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.SaaRecommends.GetAllAsync(cancellationToken);
            return entities.Select(RecommendMapper.MapRecommend);
        }
    }
}

public class GetRecommendByIdQuery : IRequest<SaaRecommendDto?>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<GetRecommendByIdQuery, SaaRecommendDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<SaaRecommendDto?> Handle(GetRecommendByIdQuery request, CancellationToken cancellationToken)
        {
            var e = await _unitOfWork.SaaRecommends.GetByIdAsync(request.Id, cancellationToken);
            return e == null ? null : RecommendMapper.MapRecommend(e);
        }
    }
}

public class GetRecommendsByPeriodQuery : IRequest<IEnumerable<SaaRecommendDto>>
{
    public long PeriodId { get; set; }

    public class Handler : IRequestHandler<GetRecommendsByPeriodQuery, IEnumerable<SaaRecommendDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SaaRecommendDto>> Handle(GetRecommendsByPeriodQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.SaaRecommends.GetByPeriodAsync(request.PeriodId, cancellationToken);
            return entities.Select(RecommendMapper.MapRecommend);
        }
    }
}

public class GetRecommendsByEmployeeQuery : IRequest<IEnumerable<SaaRecommendDto>>
{
    public long EmpSysId { get; set; }

    public class Handler : IRequestHandler<GetRecommendsByEmployeeQuery, IEnumerable<SaaRecommendDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SaaRecommendDto>> Handle(GetRecommendsByEmployeeQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.SaaRecommends.GetByEmployeeAsync(request.EmpSysId, cancellationToken);
            return entities.Select(RecommendMapper.MapRecommend);
        }
    }
}

public class GetAllSubmitsQuery : IRequest<IEnumerable<SaaSubmitDto>>
{
    public class Handler : IRequestHandler<GetAllSubmitsQuery, IEnumerable<SaaSubmitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SaaSubmitDto>> Handle(GetAllSubmitsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.SaaSubmits.GetAllAsync(cancellationToken);
            return entities.Select(e => new SaaSubmitDto
            {
                Id = e.Id, PeriodId = e.PeriodId, BusId = e.BusId,
                BhrFlag = e.BhrFlag, ChrFlag = e.ChrFlag,
                BhrUpdBy = e.BhrUpdBy, BhrUpdOn = e.BhrUpdOn, BhrAmount = e.BhrAmount,
                ChrUpdBy = e.ChrUpdBy, ChrUpdOn = e.ChrUpdOn, ChrAmount = e.ChrAmount,
                CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
            });
        }
    }
}

public class GetAllMailTriggersQuery : IRequest<IEnumerable<SaaMailTriggerDto>>
{
    public class Handler : IRequestHandler<GetAllMailTriggersQuery, IEnumerable<SaaMailTriggerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SaaMailTriggerDto>> Handle(GetAllMailTriggersQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.SaaMailTriggers.GetAllAsync(cancellationToken);
            return entities.Select(e => new SaaMailTriggerDto
            {
                Id = e.Id, QuarterId = e.QuarterId, EmpSysId = e.EmpSysId,
                MailId = e.MailId, TriggeredBy = e.TriggeredBy, TriggeredOn = e.TriggeredOn,
                CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
            });
        }
    }
}

internal static class RecommendMapper
{
    public static SaaRecommendDto MapRecommend(TransactionService.Domain.Entities.SaaRecommend e) => new()
    {
        Id = e.Id, YearId = e.YearId, PeriodId = e.PeriodId, EmpSysId = e.EmpSysId,
        LevelId = e.LevelId, CtcAmount = e.CtcAmount, MaximumCap = e.MaximumCap,
        EligibilityAmount = e.EligibilityAmount, RecommendAmount = e.RecommendAmount,
        InitiativeTaken = e.InitiativeTaken, Results = e.Results, AddRemarks = e.AddRemarks,
        Status = e.Status, RejectionBy = e.RejectionBy, RejectionOn = e.RejectionOn,
        RecommendBy = e.RecommendBy, RecommendSubmitBy = e.RecommendSubmitBy,
        RecommendSubmitOn = e.RecommendSubmitOn, ReviewerSubmitBy = e.ReviewerSubmitBy,
        ReviewerSubmitOn = e.ReviewerSubmitOn, BhrSubmitBy = e.BhrSubmitBy, BhrSubmitOn = e.BhrSubmitOn,
        ChrSubmitBy = e.ChrSubmitBy, ChrSubmitOn = e.ChrSubmitOn,
        RejectionRemarks = e.RejectionRemarks, FinalLevel = e.FinalLevel, FinalAmount = e.FinalAmount,
        InitiativeLetter = e.InitiativeLetter, ResultsLetter = e.ResultsLetter,
        UhrSubmitBy = e.UhrSubmitBy, UhrSubmitOn = e.UhrSubmitOn,
        RecommendSignId = e.RecommendSignId, RecommendSignId2 = e.RecommendSignId2,
        CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
    };
}
