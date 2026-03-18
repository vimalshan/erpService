using AccountingService.Application.Common.Interfaces;
using AccountingService.Application.DTOs;
using AccountingService.Domain.Entities;
using MediatR;

namespace AccountingService.Application.Features.AccountDetails.Commands.CreateAccountDetail;

public class CreateAccountDetailCommandHandler : IRequestHandler<CreateAccountDetailCommand, AccountDetailDto>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountDetailCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<AccountDetailDto> Handle(CreateAccountDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = AccountDetail.Create(
            request.AcSysId, request.AcTrustCode, request.AcTranCode,
            request.AcTranNo, request.AcDocNo, request.AcFinYer,
            request.AcDocDat, request.AcMainCode, request.AcSubCode,
            request.AcDcType, request.AcTranAmt, request.AcRefTranCode,
            request.AcRefTranNo, request.AcRemarks);

        _context.AccountDetails.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new AccountDetailDto(entity.AcSysId, entity.AcTrustCode, entity.AcTranCode,
            entity.AcTranNo, entity.AcDocNo, entity.AcFinYer, entity.AcDocDat,
            entity.AcMainCode, entity.AcSubCode, entity.AcDcType, entity.AcTranAmt,
            entity.AcRefTranCode, entity.AcRefTranNo, entity.AcRemarks);
    }
}
