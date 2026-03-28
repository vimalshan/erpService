using MediatR;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Commands;

public class CreateBudgetCommand : IRequest<long>
{
    public long BusinessId { get; set; }
    public long YearId { get; set; }
    public decimal BudgetAmount { get; set; }
    public long UpdatedBy { get; set; }

    public class Handler : IRequestHandler<CreateBudgetCommand, long>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<long> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            var entity = new TransactionService.Domain.Entities.SaaBudget(
                request.BusinessId, request.YearId, request.BudgetAmount, request.UpdatedBy);
            var created = await _unitOfWork.SaaBudgets.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return created.Id;
        }
    }
}

public class UpdateBudgetCommand : IRequest<bool>
{
    public long Id { get; set; }
    public decimal BudgetAmount { get; set; }
    public long UpdatedBy { get; set; }

    public class Handler : IRequestHandler<UpdateBudgetCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
        {
            var budget = await _unitOfWork.SaaBudgets.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Budget {request.Id} not found");
            budget.BudgetAmount = request.BudgetAmount;
            budget.UpdatedBy = request.UpdatedBy;
            budget.UpdatedOn = DateTime.UtcNow;
            budget.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaaBudgets.UpdateAsync(budget, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

public class CreatePeriodCommand : IRequest<long>
{
    public long YearId { get; set; }
    public long QuarterNo { get; set; }
    public DateTime PeriodOpenDate { get; set; }
    public DateTime PeriodCloseDate { get; set; }
    public DateTime FormOpenDate { get; set; }
    public DateTime? AppraiserLastDate { get; set; }
    public DateTime? ReviewerLastDate { get; set; }
    public DateTime? BhrLastDate { get; set; }
    public DateTime? UhrLastDate { get; set; }

    public class Handler : IRequestHandler<CreatePeriodCommand, long>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<long> Handle(CreatePeriodCommand request, CancellationToken cancellationToken)
        {
            var entity = new TransactionService.Domain.Entities.SaaPeriod(
                request.YearId, request.QuarterNo, request.PeriodOpenDate,
                request.PeriodCloseDate, request.FormOpenDate)
            {
                AppraiserLastDate = request.AppraiserLastDate,
                ReviewerLastDate = request.ReviewerLastDate,
                BhrLastDate = request.BhrLastDate,
                UhrLastDate = request.UhrLastDate
            };
            var created = await _unitOfWork.SaaPeriods.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return created.Id;
        }
    }
}

public class CreateLevelCommand : IRequest<long>
{
    public string LevelDesc { get; set; } = string.Empty;
    public string LevelAmount { get; set; } = string.Empty;
    public string LevelReason { get; set; } = string.Empty;
    public decimal LevelMin { get; set; }
    public decimal LevelMax { get; set; }
    public DateTime LevelEffDate { get; set; }
    public long UpdatedBy { get; set; }

    public class Handler : IRequestHandler<CreateLevelCommand, long>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<long> Handle(CreateLevelCommand request, CancellationToken cancellationToken)
        {
            var entity = new TransactionService.Domain.Entities.SaaLevel(
                request.LevelDesc, request.LevelAmount, request.LevelReason,
                request.LevelMin, request.LevelMax, request.LevelEffDate, request.UpdatedBy);
            var created = await _unitOfWork.SaaLevels.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return created.Id;
        }
    }
}
