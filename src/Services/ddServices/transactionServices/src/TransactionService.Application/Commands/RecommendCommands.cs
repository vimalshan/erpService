using MediatR;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Commands;

public class CreateRecommendCommand : IRequest<long>
{
    public long YearId { get; set; }
    public long PeriodId { get; set; }
    public long EmpSysId { get; set; }
    public long LevelId { get; set; }
    public decimal CtcAmount { get; set; }
    public decimal MaximumCap { get; set; }
    public decimal EligibilityAmount { get; set; }
    public decimal? RecommendAmount { get; set; }
    public string InitiativeTaken { get; set; } = string.Empty;
    public string Results { get; set; } = string.Empty;
    public string? AddRemarks { get; set; }
    public string RecommendBy { get; set; } = string.Empty;

    public class Handler : IRequestHandler<CreateRecommendCommand, long>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<long> Handle(CreateRecommendCommand request, CancellationToken cancellationToken)
        {
            var entity = new TransactionService.Domain.Entities.SaaRecommend(
                request.YearId, request.PeriodId, request.EmpSysId, request.LevelId,
                request.CtcAmount, request.MaximumCap, request.EligibilityAmount,
                request.InitiativeTaken, request.Results, request.RecommendBy)
            {
                RecommendAmount = request.RecommendAmount,
                AddRemarks = request.AddRemarks
            };
            var created = await _unitOfWork.SaaRecommends.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return created.Id;
        }
    }
}

public class SubmitRecommendCommand : IRequest<bool>
{
    public long RecommendId { get; set; }
    public string ApproverRole { get; set; } = string.Empty;
    public long SubmittedBy { get; set; }
    public decimal? FinalAmount { get; set; }
    public long? FinalLevel { get; set; }

    public class Handler : IRequestHandler<SubmitRecommendCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(SubmitRecommendCommand request, CancellationToken cancellationToken)
        {
            var recommend = await _unitOfWork.SaaRecommends.GetByIdAsync(request.RecommendId, cancellationToken)
                ?? throw new KeyNotFoundException($"Recommendation {request.RecommendId} not found");

            var now = DateTime.UtcNow;
            switch (request.ApproverRole.ToUpperInvariant())
            {
                case "APR":
                    recommend.RecommendSubmitBy = request.SubmittedBy;
                    recommend.RecommendSubmitOn = now;
                    break;
                case "REV":
                    recommend.ReviewerSubmitBy = request.SubmittedBy;
                    recommend.ReviewerSubmitOn = now;
                    break;
                case "BHR":
                    recommend.BhrSubmitBy = request.SubmittedBy;
                    recommend.BhrSubmitOn = now;
                    break;
                case "CHR":
                    recommend.ChrSubmitBy = request.SubmittedBy;
                    recommend.ChrSubmitOn = now;
                    recommend.FinalAmount = request.FinalAmount;
                    recommend.FinalLevel = request.FinalLevel;
                    break;
                case "UHR":
                    recommend.UhrSubmitBy = request.SubmittedBy;
                    recommend.UhrSubmitOn = now;
                    break;
                default:
                    throw new ArgumentException($"Invalid approver role: {request.ApproverRole}");
            }

            recommend.UpdatedAt = now;
            await _unitOfWork.SaaRecommends.UpdateAsync(recommend, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

public class RejectRecommendCommand : IRequest<bool>
{
    public long RecommendId { get; set; }
    public long RejectedBy { get; set; }
    public string? RejectionRemarks { get; set; }

    public class Handler : IRequestHandler<RejectRecommendCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(RejectRecommendCommand request, CancellationToken cancellationToken)
        {
            var recommend = await _unitOfWork.SaaRecommends.GetByIdAsync(request.RecommendId, cancellationToken)
                ?? throw new KeyNotFoundException($"Recommendation {request.RecommendId} not found");

            recommend.RejectionBy = request.RejectedBy;
            recommend.RejectionOn = DateTime.UtcNow;
            recommend.RejectionRemarks = request.RejectionRemarks;
            recommend.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaaRecommends.UpdateAsync(recommend, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

public class CreateSubmitCommand : IRequest<long>
{
    public long PeriodId { get; set; }
    public long BusId { get; set; }
    public long BhrUpdBy { get; set; }
    public decimal? BhrAmount { get; set; }

    public class Handler : IRequestHandler<CreateSubmitCommand, long>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<long> Handle(CreateSubmitCommand request, CancellationToken cancellationToken)
        {
            var entity = new TransactionService.Domain.Entities.SaaSubmit(
                request.PeriodId, request.BusId, request.BhrUpdBy)
            {
                BhrAmount = request.BhrAmount
            };
            var created = await _unitOfWork.SaaSubmits.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return created.Id;
        }
    }
}

public class CreateMailTriggerCommand : IRequest<long>
{
    public long QuarterId { get; set; }
    public long EmpSysId { get; set; }
    public string MailId { get; set; } = string.Empty;
    public long TriggeredBy { get; set; }

    public class Handler : IRequestHandler<CreateMailTriggerCommand, long>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<long> Handle(CreateMailTriggerCommand request, CancellationToken cancellationToken)
        {
            var entity = new TransactionService.Domain.Entities.SaaMailTrigger(
                request.QuarterId, request.EmpSysId, request.MailId, request.TriggeredBy);
            var created = await _unitOfWork.SaaMailTriggers.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return created.Id;
        }
    }
}
