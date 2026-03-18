using MediatR;
using MemberService.Domain.Exceptions;
using MemberService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MemberService.Application.Commands.CloseMember;

public class CloseMemberCommandHandler : IRequestHandler<CloseMemberCommand, bool>
{
    private readonly IMemberRepository _repository;
    private readonly ILogger<CloseMemberCommandHandler> _logger;

    public CloseMemberCommandHandler(IMemberRepository repository, ILogger<CloseMemberCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(CloseMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _repository.GetByIdAsync(request.MemberNo, cancellationToken)
            ?? throw new MemberDomainException($"Member {request.MemberNo} not found.");

        member.CloseAccount(request.LeaveReason, request.LeaveDate, request.ApprovedBy);
        await _repository.UpdateAsync(member, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Member {MemberNo} account closed.", request.MemberNo);
        return true;
    }
}
