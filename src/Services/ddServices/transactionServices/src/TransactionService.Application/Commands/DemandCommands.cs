using MediatR;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Commands;

public class CreateDemandCommand : IRequest<long>
{
    public string DemandType { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string DemandDescription { get; set; } = string.Empty;
    public DateTime RequiredDate { get; set; }
    public string Priority { get; set; } = string.Empty;
    public long CreatedBy { get; set; }

    public class Handler : IRequestHandler<CreateDemandCommand, long>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<long> Handle(CreateDemandCommand request, CancellationToken cancellationToken)
        {
            var entity = new TransactionService.Domain.Entities.DemandMaster(
                request.DemandType, request.DepartmentId, request.DemandDescription,
                request.RequiredDate, request.Priority, request.CreatedBy);
            var created = await _unitOfWork.DemandMasters.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return created.Id;
        }
    }
}

public class ApproveDemandCommand : IRequest<bool>
{
    public long DemandId { get; set; }
    public char ApprovalStatus { get; set; }
    public string? ApprovalRemarks { get; set; }
    public long ApprovedBy { get; set; }

    public class Handler : IRequestHandler<ApproveDemandCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(ApproveDemandCommand request, CancellationToken cancellationToken)
        {
            var demand = await _unitOfWork.DemandMasters.GetByIdAsync(request.DemandId, cancellationToken)
                ?? throw new KeyNotFoundException($"Demand {request.DemandId} not found");

            if (request.ApprovalStatus != 'A' && request.ApprovalStatus != 'R')
                throw new ArgumentException("Invalid approval status. Must be 'A' or 'R'.");

            demand.DemandStatus = request.ApprovalStatus;
            demand.ApprovalRemarks = request.ApprovalRemarks;
            demand.ApprovedBy = request.ApprovedBy;
            demand.ApprovalDate = DateTime.UtcNow;
            demand.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.DemandMasters.UpdateAsync(demand, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

public class CompleteDemandCommand : IRequest<bool>
{
    public long DemandId { get; set; }
    public string? CompletionRemarks { get; set; }
    public long CompletedBy { get; set; }

    public class Handler : IRequestHandler<CompleteDemandCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(CompleteDemandCommand request, CancellationToken cancellationToken)
        {
            var demand = await _unitOfWork.DemandMasters.GetByIdAsync(request.DemandId, cancellationToken)
                ?? throw new KeyNotFoundException($"Demand {request.DemandId} not found");

            demand.DemandStatus = 'C';
            demand.CompletionRemarks = request.CompletionRemarks;
            demand.CompletedBy = request.CompletedBy;
            demand.CompletionDate = DateTime.UtcNow;
            demand.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.DemandMasters.UpdateAsync(demand, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
