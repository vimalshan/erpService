using MediatR;
using CanteenUnit.Application.DTOs;

namespace CanteenUnit.Application.Features.CanteenMasters.Commands;

public record CreateCanteenMasterCommand(
    decimal ComCode,
    long CanNum,
    DateTime? FromDate,
    DateTime? ToDate,
    char? LiveFlag,
    decimal? EnteredBy,
    string? Remark) : IRequest<CanteenMasterDto>;

public record UpdateCanteenMasterLiveFlagCommand(decimal ComCode, char Flag) : IRequest;
public record DeleteCanteenMasterCommand(decimal ComCode) : IRequest;
