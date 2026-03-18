using MediatR;
using SwipeTransactionService.Application.DTOs;

namespace SwipeTransactionService.Application.Features.SwipeTransactions.Commands;

public sealed record RecordSwipeUploadCommand(
    long CompanyCode,
    string EmployeeNumber,
    DateTime SwipeTime,
    long ItemCode,
    long ItemQuantity,
    long BatchNumber,
    long SerialNumber,
    char CanteenNumber,
    string GateNumber) : IRequest<SwipeCardUploadDto>;
