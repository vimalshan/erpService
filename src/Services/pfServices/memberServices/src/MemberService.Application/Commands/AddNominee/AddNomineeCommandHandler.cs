using AutoMapper;
using MediatR;
using MemberService.Application.DTOs;
using MemberService.Domain.Exceptions;
using MemberService.Domain.Interfaces;

namespace MemberService.Application.Commands.AddNominee;

public class AddNomineeCommandHandler : IRequestHandler<AddNomineeCommand, NomineeDto>
{
    private readonly IMemberRepository _repository;
    private readonly IMapper _mapper;

    public AddNomineeCommandHandler(IMemberRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NomineeDto> Handle(AddNomineeCommand request, CancellationToken cancellationToken)
    {
        var member = await _repository.GetByIdAsync(request.MemberNo, cancellationToken)
            ?? throw new MemberDomainException($"Member {request.MemberNo} not found.");

        var nominee = member.AddNominee(request.SerialNo, request.FundType, request.NomineeName,
            request.RelationshipCode, request.Percentage, request.DateOfBirth, request.IsMinor,
            request.AddressLine1, request.PhoneNo, request.Email);

        await _repository.UpdateAsync(member, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NomineeDto>(nominee);
    }
}
