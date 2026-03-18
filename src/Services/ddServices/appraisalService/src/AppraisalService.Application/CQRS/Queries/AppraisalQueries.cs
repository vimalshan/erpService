using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using AppraisalService.Domain.Repositories;
using AppraisalService.Application.DTOs;

namespace AppraisalService.Application.CQRS.Queries;

// Get Appraisal by Request Number
public class GetAppraisalByRequestQuery : IRequest<AppraisalDetailedDto?>
{
    public long RequestNumber { get; set; }

    public GetAppraisalByRequestQuery(long requestNumber)
    {
        RequestNumber = requestNumber;
    }
}

public class GetAppraisalByRequestQueryHandler : IRequestHandler<GetAppraisalByRequestQuery, AppraisalDetailedDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAppraisalByRequestQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AppraisalDetailedDto?> Handle(GetAppraisalByRequestQuery request, CancellationToken cancellationToken)
    {
        var appraisal = await _unitOfWork.Appraisals.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        if (appraisal == null) return null;

        var dto = _mapper.Map<AppraisalDetailedDto>(appraisal);
        return dto;
    }
}

// Get Appraisal by User Code
public class GetAppraisalByUserQuery : IRequest<AppraisalMainDto?>
{
    public string UserCode { get; set; }

    public GetAppraisalByUserQuery(string userCode)
    {
        UserCode = userCode;
    }
}

public class GetAppraisalByUserQueryHandler : IRequestHandler<GetAppraisalByUserQuery, AppraisalMainDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAppraisalByUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AppraisalMainDto?> Handle(GetAppraisalByUserQuery request, CancellationToken cancellationToken)
    {
        var appraisal = await _unitOfWork.Appraisals.GetByUserCodeAsync(request.UserCode, cancellationToken);
        if (appraisal == null) return null;

        return _mapper.Map<AppraisalMainDto>(appraisal);
    }
}

// Get All Appraisals by Year
public class GetAppraisalsByYearQuery : IRequest<IEnumerable<AppraisalMainDto>>
{
    public long YearId { get; set; }

    public GetAppraisalsByYearQuery(long yearId)
    {
        YearId = yearId;
    }
}

public class GetAppraisalsByYearQueryHandler : IRequestHandler<GetAppraisalsByYearQuery, IEnumerable<AppraisalMainDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAppraisalsByYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppraisalMainDto>> Handle(GetAppraisalsByYearQuery request, CancellationToken cancellationToken)
    {
        var appraisals = await _unitOfWork.Appraisals.GetByYearAsync(request.YearId, cancellationToken);
        return _mapper.Map<IEnumerable<AppraisalMainDto>>(appraisals);
    }
}

// Get Appraisals by Status
public class GetAppraisalsByStatusQuery : IRequest<IEnumerable<AppraisalMainDto>>
{
    public string StatusCode { get; set; }

    public GetAppraisalsByStatusQuery(string statusCode)
    {
        StatusCode = statusCode;
    }
}

public class GetAppraisalsByStatusQueryHandler : IRequestHandler<GetAppraisalsByStatusQuery, IEnumerable<AppraisalMainDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAppraisalsByStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppraisalMainDto>> Handle(GetAppraisalsByStatusQuery request, CancellationToken cancellationToken)
    {
        var appraisals = await _unitOfWork.Appraisals.GetByStatusAsync(request.StatusCode, cancellationToken);
        return _mapper.Map<IEnumerable<AppraisalMainDto>>(appraisals);
    }
}

// Get Competency Assessments by Request
public class GetCompetencyAssessmentsQuery : IRequest<IEnumerable<CompetencyAssessmentDto>>
{
    public long RequestNumber { get; set; }

    public GetCompetencyAssessmentsQuery(long requestNumber)
    {
        RequestNumber = requestNumber;
    }
}

public class GetCompetencyAssessmentsQueryHandler : IRequestHandler<GetCompetencyAssessmentsQuery, IEnumerable<CompetencyAssessmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompetencyAssessmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompetencyAssessmentDto>> Handle(GetCompetencyAssessmentsQuery request, CancellationToken cancellationToken)
    {
        var assessments = await _unitOfWork.CompetencyAssessments.GetByRequestAsync(request.RequestNumber, cancellationToken);
        return _mapper.Map<IEnumerable<CompetencyAssessmentDto>>(assessments);
    }
}

// Get Appraisal Bands
public class GetAppraisalBandsQuery : IRequest<IEnumerable<AppraisalBandDto>>
{
    public GetAppraisalBandsQuery()
    {
    }
}

public class GetAppraisalBandsQueryHandler : IRequestHandler<GetAppraisalBandsQuery, IEnumerable<AppraisalBandDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAppraisalBandsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppraisalBandDto>> Handle(GetAppraisalBandsQuery request, CancellationToken cancellationToken)
    {
        var bands = await _unitOfWork.AppraisalBands.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AppraisalBandDto>>(bands);
    }
}
