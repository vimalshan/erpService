using MediatR;
using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.Application.Queries.GetAccumulations;

public record GetAccumulationsQuery : IRequest<IEnumerable<PFAccumulationDto>>;
