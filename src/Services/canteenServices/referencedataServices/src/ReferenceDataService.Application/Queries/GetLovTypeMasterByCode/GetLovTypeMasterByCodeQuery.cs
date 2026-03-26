using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Queries.GetLovTypeMasterByCode;

public record GetLovTypeMasterByCodeQuery(string LovTypeCode) : IRequest<LovTypeMasterDto?>;
