using LovService.Application.DTOs;
using MediatR;

namespace LovService.Application.Queries.LovMaster;

public record GetAllLovMastersQuery : IRequest<IEnumerable<LovMasterDto>>;

public record GetLovMasterByIdQuery(long LovId) : IRequest<LovMasterDto?>;

public record GetLovMastersByTypeQuery(long LovTypeId) : IRequest<IEnumerable<LovMasterDto>>;
