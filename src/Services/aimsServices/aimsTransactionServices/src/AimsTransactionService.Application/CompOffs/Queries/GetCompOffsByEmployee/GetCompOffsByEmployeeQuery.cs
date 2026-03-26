using MediatR;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.CompOffs.Queries.GetCompOffsByEmployee;

public sealed record GetCompOffsByEmployeeQuery(long EmployeeSysId) : IRequest<IEnumerable<CompOffDto>>;
