using MediatR;
using TdsService.Application.DTOs;

namespace TdsService.Application.Vendors.Queries.GetTdsVendorByPan;

public sealed record GetTdsVendorByPanQuery(string PanNo) : IRequest<TdsVendorDto?>;
