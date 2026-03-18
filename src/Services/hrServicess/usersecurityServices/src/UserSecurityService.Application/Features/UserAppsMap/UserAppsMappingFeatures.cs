using FluentValidation;
using MediatR;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Application.Mappings;
using UserSecurityService.Domain.Interfaces;
using UserAppsMapEntity = UserSecurityService.Domain.Entities.UserAppsMap;

namespace UserSecurityService.Application.Features.UserAppsMap.Commands;

public record CreateUserAppsMappingCommand(
    decimal EmpSysId,
    string AppCode,
    DateTime EffectiveDate,
    decimal HrRoleId,
    decimal CreatedBy,
    string? Remarks = null
) : IRequest<UserAppsMappingDto>;

public sealed class CreateUserAppsMappingCommandValidator : AbstractValidator<CreateUserAppsMappingCommand>
{
    public CreateUserAppsMappingCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.AppCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.HrRoleId).GreaterThan(0);
    }
}

public sealed class CreateUserAppsMappingCommandHandler(
    IUserAppsMappingRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateUserAppsMappingCommand, UserAppsMappingDto>
{
    public async Task<UserAppsMappingDto> Handle(CreateUserAppsMappingCommand cmd, CancellationToken ct)
    {
        var entity = UserAppsMapEntity.Create(
            cmd.EmpSysId, cmd.AppCode, cmd.EffectiveDate,
            cmd.HrRoleId, cmd.CreatedBy, cmd.Remarks);

        await repository.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return entity.ToDto();
    }
}

// Query
public record GetUserAppsMappingQuery(decimal EmpSysId) : IRequest<UserAppsMappingDto?>;

public sealed class GetUserAppsMappingQueryHandler(
    IUserAppsMappingRepository repository)
    : IRequestHandler<GetUserAppsMappingQuery, UserAppsMappingDto?>
{
    public async Task<UserAppsMappingDto?> Handle(GetUserAppsMappingQuery request, CancellationToken ct)
    {
        var entity = await repository.GetByEmpSysIdAsync(request.EmpSysId, ct);
        return entity?.ToDto();
    }
}
