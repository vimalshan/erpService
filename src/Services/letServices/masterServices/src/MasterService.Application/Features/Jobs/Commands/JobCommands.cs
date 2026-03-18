using MediatR;
using MasterService.Application.DTOs;

namespace MasterService.Application.Features.Jobs.Commands;

public record CreateJobCommand(long JobCode, string JobName, string CategoryCode, long? SerialNumber) : IRequest<JobMasterDto>;
public record UpdateJobCommand(long JobCode, string JobName, string CategoryCode) : IRequest<JobMasterDto>;
