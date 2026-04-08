using MediatR;
using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.Application.Queries.GetAccumulation;

public record GetAccumulationQuery(long EmpSysId) : IRequest<PFAccumulationDto?>;
