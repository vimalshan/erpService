using MediatR;
using MasterService.Application.DTOs;

namespace MasterService.Application.Features.Jobs.Queries;

public record GetJobsQuery(string? CategoryCode = null) : IRequest<IEnumerable<JobMasterDto>>;
public record GetJobByCodeQuery(long JobCode) : IRequest<JobMasterDto?>;
