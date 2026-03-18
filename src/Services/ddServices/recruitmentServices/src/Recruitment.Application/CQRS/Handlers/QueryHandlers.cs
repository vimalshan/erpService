using AutoMapper;
using MediatR;
using Recruitment.Application.CQRS.Queries;
using Recruitment.Application.DTOs;
using Recruitment.Domain.Repositories;

namespace Recruitment.Application.CQRS.Handlers;

/// <summary>
/// Handler for GetJobByIdQuery
/// </summary>
public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, JobDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetJobByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<JobDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId);
        return _mapper.Map<JobDto>(job);
    }
}

/// <summary>
/// Handler for GetAllJobsQuery
/// </summary>
public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery, IEnumerable<JobDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllJobsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<JobDto>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _unitOfWork.Jobs.GetAllAsync();
        return _mapper.Map<IEnumerable<JobDto>>(jobs);
    }
}

/// <summary>
/// Handler for GetActiveJobsQuery
/// </summary>
public class GetActiveJobsQueryHandler : IRequestHandler<GetActiveJobsQuery, IEnumerable<JobDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActiveJobsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<JobDto>> Handle(GetActiveJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _unitOfWork.Jobs.GetActiveJobsAsync();
        return _mapper.Map<IEnumerable<JobDto>>(jobs);
    }
}

/// <summary>
/// Handler for GetApplicationByIdQuery
/// </summary>
public class GetApplicationByIdQueryHandler : IRequestHandler<GetApplicationByIdQuery, ApplicationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetApplicationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationDto> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
    {
        var application = await _unitOfWork.Applications.GetByIdAsync(request.ApplicationNumber);
        return _mapper.Map<ApplicationDto>(application);
    }
}

/// <summary>
/// Handler for GetAllApplicationsQuery
/// </summary>
public class GetAllApplicationsQueryHandler : IRequestHandler<GetAllApplicationsQuery, IEnumerable<ApplicationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllApplicationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApplicationDto>> Handle(GetAllApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applications = await _unitOfWork.Applications.GetAllAsync();
        return _mapper.Map<IEnumerable<ApplicationDto>>(applications);
    }
}

/// <summary>
/// Handler for GetApplicationsByJobIdQuery
/// </summary>
public class GetApplicationsByJobIdQueryHandler : IRequestHandler<GetApplicationsByJobIdQuery, IEnumerable<ApplicationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetApplicationsByJobIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApplicationDto>> Handle(GetApplicationsByJobIdQuery request, CancellationToken cancellationToken)
    {
        var applications = await _unitOfWork.Applications.GetAllByJobIdAsync(request.JobId);
        return _mapper.Map<IEnumerable<ApplicationDto>>(applications);
    }
}

/// <summary>
/// Handler for GetApplicationsBySparshIdQuery
/// </summary>
public class GetApplicationsBySparshIdQueryHandler : IRequestHandler<GetApplicationsBySparshIdQuery, IEnumerable<ApplicationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetApplicationsBySparshIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApplicationDto>> Handle(GetApplicationsBySparshIdQuery request, CancellationToken cancellationToken)
    {
        var applications = await _unitOfWork.Applications.GetAllBySparshIdAsync(request.SparshId);
        return _mapper.Map<IEnumerable<ApplicationDto>>(applications);
    }
}
