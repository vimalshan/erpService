using MediatR;
using CanteenUnit.Application.DTOs;

namespace CanteenUnit.Application.Features.CanteenMasters.Queries;

public record GetAllCanteenMastersQuery : IRequest<IEnumerable<CanteenMasterDto>>;
public record GetCanteenMasterQuery(decimal ComCode) : IRequest<CanteenMasterDto?>;
