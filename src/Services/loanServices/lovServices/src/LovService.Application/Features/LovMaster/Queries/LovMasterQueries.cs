using MediatR;
using LovService.Application.DTOs;

namespace LovService.Application.Features.LovMaster.Queries;

public record GetLovMasterByIdQuery(long LovId) : IRequest<LovMasterDto?>;
public record GetAllLovMastersQuery(int? LovTypeId = null) : IRequest<IEnumerable<LovMasterDto>>;
