using MediatR;
using AutoMapper;
using LoanApplication.Application.Queries;
using LoanApplication.Application.DTOs;
using LoanApplication.Domain.Interfaces;

namespace LoanApplication.Application.QueryHandlers;

/// <summary>
/// Handler for GetLoanApplicationByIdQuery
/// </summary>
public class GetLoanApplicationByIdQueryHandler : IRequestHandler<GetLoanApplicationByIdQuery, LoanApplicationDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLoanApplicationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoanApplicationDto?> Handle(GetLoanApplicationByIdQuery request, CancellationToken cancellationToken)
    {
        var loanApplication = await _unitOfWork.LoanApplications.GetByIdAsync(request.LoanApplicationId, cancellationToken);
        return loanApplication != null ? _mapper.Map<LoanApplicationDto>(loanApplication) : null;
    }
}

/// <summary>
/// Handler for GetLoanApplicationsByEmployeeIdQuery
/// </summary>
public class GetLoanApplicationsByEmployeeIdQueryHandler : IRequestHandler<GetLoanApplicationsByEmployeeIdQuery, List<LoanApplicationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLoanApplicationsByEmployeeIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<LoanApplicationDto>> Handle(GetLoanApplicationsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var loanApplications = await _unitOfWork.LoanApplications.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        return _mapper.Map<List<LoanApplicationDto>>(loanApplications);
    }
}

/// <summary>
/// Handler for GetAllLoanApplicationsQuery
/// </summary>
public class GetAllLoanApplicationsQueryHandler : IRequestHandler<GetAllLoanApplicationsQuery, List<LoanApplicationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllLoanApplicationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<LoanApplicationDto>> Handle(GetAllLoanApplicationsQuery request, CancellationToken cancellationToken)
    {
        var loanApplications = await _unitOfWork.LoanApplications.GetAllAsync(cancellationToken);
        return _mapper.Map<List<LoanApplicationDto>>(loanApplications);
    }
}

/// <summary>
/// Handler for GetPendingLoanApplicationsQuery
/// </summary>
public class GetPendingLoanApplicationsQueryHandler : IRequestHandler<GetPendingLoanApplicationsQuery, List<LoanApplicationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPendingLoanApplicationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<LoanApplicationDto>> Handle(GetPendingLoanApplicationsQuery request, CancellationToken cancellationToken)
    {
        var loanApplications = await _unitOfWork.LoanApplications.GetPendingAsync(cancellationToken);
        return _mapper.Map<List<LoanApplicationDto>>(loanApplications);
    }
}

/// <summary>
/// Handler for CheckLoanEligibilityQuery
/// </summary>
public class CheckLoanEligibilityQueryHandler : IRequestHandler<CheckLoanEligibilityQuery, EligibilityCheckDto>
{
    private readonly ILoanEligibilityService _eligibilityService;
    private readonly IMapper _mapper;

    public CheckLoanEligibilityQueryHandler(ILoanEligibilityService eligibilityService, IMapper mapper)
    {
        _eligibilityService = eligibilityService;
        _mapper = mapper;
    }

    public async Task<EligibilityCheckDto> Handle(CheckLoanEligibilityQuery request, CancellationToken cancellationToken)
    {
        var result = await _eligibilityService.GetEligibilityDetailsAsync(request.EmployeeId, request.LoanTypeId, cancellationToken);
        return _mapper.Map<EligibilityCheckDto>(result);
    }
}
