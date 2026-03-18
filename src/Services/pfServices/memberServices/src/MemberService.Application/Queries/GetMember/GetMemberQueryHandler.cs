using AutoMapper;
using MediatR;
using MemberService.Application.DTOs;
using MemberService.Domain.Interfaces;

namespace MemberService.Application.Queries.GetMember;

public class GetMemberQueryHandler :
    IRequestHandler<GetMemberQuery, MemberProfileDto?>,
    IRequestHandler<GetMemberByEmployeeQuery, MemberDto?>,
    IRequestHandler<GetAllMembersQuery, IReadOnlyList<MemberSummaryDto>>
{
    private readonly IMemberRepository _repository;
    private readonly IMapper _mapper;

    public GetMemberQueryHandler(IMemberRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MemberProfileDto?> Handle(GetMemberQuery request, CancellationToken cancellationToken)
    {
        var member = await _repository.GetByIdAsync(request.MemberNo, cancellationToken);
        if (member is null) return null;

        return new MemberProfileDto(
            _mapper.Map<MemberDto>(member),
            member.Nominees.Select(n => _mapper.Map<NomineeDto>(n)).ToList(),
            member.Contacts.Select(c => _mapper.Map<ContactDto>(c)).ToList()
        );
    }

    public async Task<MemberDto?> Handle(GetMemberByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var member = await _repository.GetByEmployeeSysIdAsync(request.EmployeeSysId, cancellationToken);
        return member is null ? null : _mapper.Map<MemberDto>(member);
    }

    public async Task<IReadOnlyList<MemberSummaryDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var members = string.IsNullOrWhiteSpace(request.TrustCode)
            ? await _repository.GetAllActiveAsync(cancellationToken)
            : await _repository.GetByTrustCodeAsync(request.TrustCode, cancellationToken);

        return members.Select(m => _mapper.Map<MemberSummaryDto>(m)).ToList();
    }
}
