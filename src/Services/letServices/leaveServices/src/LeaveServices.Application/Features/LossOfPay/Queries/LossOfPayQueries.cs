using MediatR;
using LeaveServices.Application.DTOs;

namespace LeaveServices.Application.Features.LossOfPay.Queries;

public record GetLossOfPayByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<LossOfPayDto>>;
