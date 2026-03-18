using AutoMapper;
using CompensationBenefits.Application.DTOs;
using CompensationBenefits.Domain.Entities;
using CompensationBenefits.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CompensationBenefits.Application.Features.MobileConnections;

public record GetMobileConnectionsByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<MobileConnectionDto>>;

public class GetMobileConnectionsByEmployeeQueryHandler(IMobileConnectionRepository repo, IMapper mapper)
    : IRequestHandler<GetMobileConnectionsByEmployeeQuery, IEnumerable<MobileConnectionDto>>
{
    public async Task<IEnumerable<MobileConnectionDto>> Handle(GetMobileConnectionsByEmployeeQuery request, CancellationToken ct)
        => mapper.Map<IEnumerable<MobileConnectionDto>>(await repo.GetByEmployeeAsync(request.EmpSysId, ct));
}

public record CreateMobileConnectionCommand(
    long ConnId,
    long EmpSysId,
    string Type,
    long PhoneNo,
    long CalendarId,
    long CreatedBy,
    DateTime EffDate) : IRequest<long>;

public class CreateMobileConnectionCommandValidator : AbstractValidator<CreateMobileConnectionCommand>
{
    public CreateMobileConnectionCommandValidator()
    {
        RuleFor(x => x.ConnId).GreaterThan(0);
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.PhoneNo).GreaterThan(0);
        RuleFor(x => x.Type).NotEmpty().MaximumLength(1);
    }
}

public class CreateMobileConnectionCommandHandler(IMobileConnectionRepository repo)
    : IRequestHandler<CreateMobileConnectionCommand, long>
{
    public async Task<long> Handle(CreateMobileConnectionCommand request, CancellationToken ct)
    {
        var conn = MobileConnection.Create(
            request.ConnId, request.EmpSysId, request.Type,
            request.PhoneNo, request.CalendarId, request.CreatedBy, request.EffDate);

        await repo.AddAsync(conn, ct);
        await repo.SaveChangesAsync(ct);
        return conn.ConnId;
    }
}
