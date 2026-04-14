using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.CancelSalesOrder;

public record CancelSalesOrderCommand(int SoId) : IRequest<SalesOrderDto>;
