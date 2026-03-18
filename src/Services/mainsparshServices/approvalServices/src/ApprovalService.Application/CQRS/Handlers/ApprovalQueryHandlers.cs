namespace ApprovalService.Application.CQRS.Handlers;

using MediatR;
using AutoMapper;
using Dapper;
using System.Data;
using ApprovalService.Application.CQRS.Queries;
using ApprovalService.Application.DTOs;
using ApprovalService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for getting approval master by ID query
/// </summary>
public class GetApprovalMasterByIdHandler : IRequestHandler<GetApprovalMasterByIdQuery, ApprovalMasterDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetApprovalMasterByIdHandler> _logger;

    public GetApprovalMasterByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetApprovalMasterByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApprovalMasterDto?> Handle(
        GetApprovalMasterByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approval = await _unitOfWork.ApprovalMasters.GetByIdAsync(request.Id);
            return approval != null ? _mapper.Map<ApprovalMasterDto>(approval) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approval master by ID");
            throw;
        }
    }
}

/// <summary>
/// Handler for getting approval master by code query
/// </summary>
public class GetApprovalMasterByCodeHandler : IRequestHandler<GetApprovalMasterByCodeQuery, ApprovalMasterDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetApprovalMasterByCodeHandler> _logger;

    public GetApprovalMasterByCodeHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetApprovalMasterByCodeHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApprovalMasterDto?> Handle(
        GetApprovalMasterByCodeQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approval = await _unitOfWork.ApprovalMasters.GetByCodeAsync(request.Code);
            return approval != null ? _mapper.Map<ApprovalMasterDto>(approval) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approval master by code");
            throw;
        }
    }
}

/// <summary>
/// Handler for getting approvals by module query
/// </summary>
public class GetApprovalsByModuleHandler : IRequestHandler<GetApprovalsByModuleQuery, List<ApprovalMasterDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetApprovalsByModuleHandler> _logger;

    public GetApprovalsByModuleHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetApprovalsByModuleHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ApprovalMasterDto>> Handle(
        GetApprovalsByModuleQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approvals = await _unitOfWork.ApprovalMasters.GetByModuleAsync(request.Module);
            return _mapper.Map<List<ApprovalMasterDto>>(approvals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approvals by module");
            throw;
        }
    }
}

/// <summary>
/// Handler for getting all approvals query
/// </summary>
public class GetAllApprovalsHandler : IRequestHandler<GetAllApprovalsQuery, List<ApprovalMasterDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllApprovalsHandler> _logger;

    public GetAllApprovalsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllApprovalsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ApprovalMasterDto>> Handle(
        GetAllApprovalsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approvals = await _unitOfWork.ApprovalMasters.GetAllAsync();
            return _mapper.Map<List<ApprovalMasterDto>>(approvals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all approvals");
            throw;
        }
    }
}

/// <summary>
/// Handler for getting approver employee by ID query
/// </summary>
public class GetApproverEmployeeByIdHandler : IRequestHandler<GetApproverEmployeeByIdQuery, ApproverEmployeeDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetApproverEmployeeByIdHandler> _logger;

    public GetApproverEmployeeByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetApproverEmployeeByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApproverEmployeeDto?> Handle(
        GetApproverEmployeeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approver = await _unitOfWork.ApproverEmployees.GetByIdAsync(request.Id);
            return approver != null ? _mapper.Map<ApproverEmployeeDto>(approver) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approver employee by ID");
            throw;
        }
    }
}

/// <summary>
/// Handler for getting approvers by approval master query
/// </summary>
public class GetApproversByApprovalMasterHandler : IRequestHandler<GetApproversByApprovalMasterQuery, List<ApproverEmployeeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetApproversByApprovalMasterHandler> _logger;

    public GetApproversByApprovalMasterHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetApproversByApprovalMasterHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ApproverEmployeeDto>> Handle(
        GetApproversByApprovalMasterQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approvers = await _unitOfWork.ApproverEmployees.GetByApprovalMasterAsync(request.ApprovalMasterId);
            return _mapper.Map<List<ApproverEmployeeDto>>(approvers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approvers by approval master");
            throw;
        }
    }
}

/// <summary>
/// Handler for getting approvers by employee query
/// </summary>
public class GetApproversByEmployeeHandler : IRequestHandler<GetApproversByEmployeeQuery, List<ApproverEmployeeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetApproversByEmployeeHandler> _logger;

    public GetApproversByEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetApproversByEmployeeHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ApproverEmployeeDto>> Handle(
        GetApproversByEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approvers = await _unitOfWork.ApproverEmployees.GetByEmployeeAsync(request.EmployeeSysId);
            return _mapper.Map<List<ApproverEmployeeDto>>(approvers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approvers by employee");
            throw;
        }
    }
}
