using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Queries.GetAllLovMasters;

public record GetAllLovMastersQuery : IRequest<IEnumerable<LovMasterDto>>;
