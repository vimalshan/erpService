using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Queries.GetAllLovTypeMasters;

public record GetAllLovTypeMastersQuery : IRequest<IEnumerable<LovTypeMasterDto>>;
