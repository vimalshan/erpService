using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Queries;

public class GetAllDemandsQuery : IRequest<IEnumerable<DemandMasterDto>>
{
    public class Handler : IRequestHandler<GetAllDemandsQuery, IEnumerable<DemandMasterDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<DemandMasterDto>> Handle(GetAllDemandsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.DemandMasters.GetAllAsync(cancellationToken);
            return entities.Select(e => new DemandMasterDto
            {
                Id = e.Id, DemandType = e.DemandType, DepartmentId = e.DepartmentId,
                DemandDescription = e.DemandDescription, RequiredDate = e.RequiredDate,
                Priority = e.Priority, DemandStatus = e.DemandStatus, CreatedBy = e.CreatedBy,
                CreatedOn = e.CreatedOn, ApprovalRemarks = e.ApprovalRemarks, ApprovedBy = e.ApprovedBy,
                ApprovalDate = e.ApprovalDate, CompletionRemarks = e.CompletionRemarks,
                CompletedBy = e.CompletedBy, CompletionDate = e.CompletionDate,
                CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
            });
        }
    }
}

public class GetDemandByIdQuery : IRequest<DemandMasterDto?>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<GetDemandByIdQuery, DemandMasterDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<DemandMasterDto?> Handle(GetDemandByIdQuery request, CancellationToken cancellationToken)
        {
            var e = await _unitOfWork.DemandMasters.GetByIdAsync(request.Id, cancellationToken);
            if (e == null) return null;
            return new DemandMasterDto
            {
                Id = e.Id, DemandType = e.DemandType, DepartmentId = e.DepartmentId,
                DemandDescription = e.DemandDescription, RequiredDate = e.RequiredDate,
                Priority = e.Priority, DemandStatus = e.DemandStatus, CreatedBy = e.CreatedBy,
                CreatedOn = e.CreatedOn, ApprovalRemarks = e.ApprovalRemarks, ApprovedBy = e.ApprovedBy,
                ApprovalDate = e.ApprovalDate, CompletionRemarks = e.CompletionRemarks,
                CompletedBy = e.CompletedBy, CompletionDate = e.CompletionDate,
                CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
            };
        }
    }
}

public class GetDemandsByStatusQuery : IRequest<IEnumerable<DemandMasterDto>>
{
    public char Status { get; set; }

    public class Handler : IRequestHandler<GetDemandsByStatusQuery, IEnumerable<DemandMasterDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<DemandMasterDto>> Handle(GetDemandsByStatusQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.DemandMasters.GetByStatusAsync(request.Status, cancellationToken);
            return entities.Select(e => new DemandMasterDto
            {
                Id = e.Id, DemandType = e.DemandType, DepartmentId = e.DepartmentId,
                DemandDescription = e.DemandDescription, RequiredDate = e.RequiredDate,
                Priority = e.Priority, DemandStatus = e.DemandStatus, CreatedBy = e.CreatedBy,
                CreatedOn = e.CreatedOn, ApprovalRemarks = e.ApprovalRemarks, ApprovedBy = e.ApprovedBy,
                ApprovalDate = e.ApprovalDate, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
            });
        }
    }
}

public class GetDemandStatusCountQuery : IRequest<int>
{
    public char Status { get; set; }

    public class Handler : IRequestHandler<GetDemandStatusCountQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<int> Handle(GetDemandStatusCountQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.DemandMasters.GetStatusCountAsync(request.Status, cancellationToken);
        }
    }
}
