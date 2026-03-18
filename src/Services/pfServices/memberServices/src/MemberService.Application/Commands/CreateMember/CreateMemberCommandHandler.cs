using AutoMapper;
using MediatR;
using MemberService.Application.DTOs;
using MemberService.Domain.Aggregates;
using MemberService.Domain.Exceptions;
using MemberService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MemberService.Application.Commands.CreateMember;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, MemberDto>
{
    private readonly IMemberRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateMemberCommandHandler> _logger;

    public CreateMemberCommandHandler(IMemberRepository repository, IMapper mapper,
        ILogger<CreateMemberCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MemberDto> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new member for EmployeeSysId {EmpSysId}", request.EmployeeSysId);

        if (await _repository.ExistsByEmployeeSysIdAsync(request.EmployeeSysId, cancellationToken))
            throw new MemberDomainException($"Active member already exists for employee {request.EmployeeSysId}.");

        var memberNo = await _repository.GetNextMemberNumberAsync(cancellationToken);

        var member = Member.Create(memberNo, request.MemberName, request.TrustCode,
            request.DateOfJoining, request.DateOfBirth, request.EmployeeType,
            request.EmployeeSysId, request.UnitCode, request.EmployeeNo, request.CreatedBy);

        await _repository.AddAsync(member, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Member {MemberNo} created successfully.", member.MemberNo);
        return _mapper.Map<MemberDto>(member);
    }
}
