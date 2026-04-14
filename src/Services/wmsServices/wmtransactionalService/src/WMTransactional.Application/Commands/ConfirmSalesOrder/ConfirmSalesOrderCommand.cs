using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.ConfirmSalesOrder;

public record ConfirmSalesOrderCommand(int SoId) : IRequest<SalesOrderDto>;
