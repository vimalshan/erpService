using AutoMapper;
using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.ApprovalWorkflows.Queries;

public class GetWorkflowByIdQueryHandler : IRequestHandler<GetWorkflowByIdQuery, ApprovalWorkflowDto?>
{
    private readonly IApprovalWorkflowRepository _repository;
    private readonly IMapper _mapper;

    public GetWorkflowByIdQueryHandler(IApprovalWorkflowRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApprovalWorkflowDto?> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.WorkflowId, cancellationToken);
        return entity is null ? null : _mapper.Map<ApprovalWorkflowDto>(entity);
    }
}

public class GetWorkflowByCodeQueryHandler : IRequestHandler<GetWorkflowByCodeQuery, ApprovalWorkflowDto?>
{
    private readonly IApprovalWorkflowRepository _repository;
    private readonly IMapper _mapper;

    public GetWorkflowByCodeQueryHandler(IApprovalWorkflowRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApprovalWorkflowDto?> Handle(GetWorkflowByCodeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByCodeAsync(request.WorkflowCode, cancellationToken);
        return entity is null ? null : _mapper.Map<ApprovalWorkflowDto>(entity);
    }
}

public class GetPendingWorkflowsQueryHandler : IRequestHandler<GetPendingWorkflowsQuery, IEnumerable<ApprovalWorkflowDto>>
{
    private readonly IApprovalWorkflowRepository _repository;
    private readonly IMapper _mapper;

    public GetPendingWorkflowsQueryHandler(IApprovalWorkflowRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApprovalWorkflowDto>> Handle(GetPendingWorkflowsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetPendingByApproverAsync(request.ApproverId, cancellationToken);
        return _mapper.Map<IEnumerable<ApprovalWorkflowDto>>(entities);
    }
}

public class GetWorkflowsByEntityQueryHandler : IRequestHandler<GetWorkflowsByEntityQuery, IEnumerable<ApprovalWorkflowDto>>
{
    private readonly IApprovalWorkflowRepository _repository;
    private readonly IMapper _mapper;

    public GetWorkflowsByEntityQueryHandler(IApprovalWorkflowRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApprovalWorkflowDto>> Handle(GetWorkflowsByEntityQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByEntityAsync(request.EntityType, request.EntityId, cancellationToken);
        return _mapper.Map<IEnumerable<ApprovalWorkflowDto>>(entities);
    }
}

public class GetAllWorkflowsQueryHandler : IRequestHandler<GetAllWorkflowsQuery, IEnumerable<ApprovalWorkflowDto>>
{
    private readonly IApprovalWorkflowRepository _repository;
    private readonly IMapper _mapper;

    public GetAllWorkflowsQueryHandler(IApprovalWorkflowRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApprovalWorkflowDto>> Handle(GetAllWorkflowsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ApprovalWorkflowDto>>(entities);
    }
}
