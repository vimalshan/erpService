using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Queries.GetLovMasterById;

public record GetLovMasterByIdQuery(string LovId) : IRequest<LovMasterDto?>;
