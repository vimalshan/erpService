using AccountingService.Application.DTOs;
using MediatR;

namespace AccountingService.Application.Features.AccountDetails.Commands.CreateAccountDetail;

public record CreateAccountDetailCommand(
    long AcSysId,
    string AcTrustCode,
    string AcTranCode,
    long AcTranNo,
    long AcDocNo,
    long AcFinYer,
    DateTime AcDocDat,
    string AcMainCode,
    string AcSubCode,
    string AcDcType,
    decimal AcTranAmt,
    string AcRefTranCode,
    long AcRefTranNo,
    string? AcRemarks
) : IRequest<AccountDetailDto>;
