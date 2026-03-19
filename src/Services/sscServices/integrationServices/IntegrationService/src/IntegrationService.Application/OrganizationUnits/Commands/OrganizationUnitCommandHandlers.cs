using AutoMapper;
using IntegrationService.Application.DTOs;
using IntegrationService.Domain.Entities;
using IntegrationService.Domain.Exceptions;
using IntegrationService.Domain.Interfaces;
using MediatR;

namespace IntegrationService.Application.OrganizationUnits.Commands;

public class CreateOrganizationUnitHandler(
    IOrganizationUnitRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateOrganizationUnitCommand, OrganizationUnitDto>
{
    public async Task<OrganizationUnitDto> Handle(CreateOrganizationUnitCommand request, CancellationToken cancellationToken)
    {
        var ou = OrganizationUnit.Create(request.OuId, request.OuName, request.BuId);
        await repository.AddAsync(ou, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<OrganizationUnitDto>(ou);
    }
}

public class UpdateOrganizationUnitHandler(
    IOrganizationUnitRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateOrganizationUnitCommand, OrganizationUnitDto>
{
    public async Task<OrganizationUnitDto> Handle(UpdateOrganizationUnitCommand request, CancellationToken cancellationToken)
    {
        var ou = await repository.GetByIdAsync(request.OuId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(OrganizationUnit), request.OuId);

        ou.UpdateDetails(request.OuName, request.BuId);
        await repository.UpdateAsync(ou, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<OrganizationUnitDto>(ou);
    }
}

public class DeleteOrganizationUnitHandler(
    IOrganizationUnitRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrganizationUnitCommand, bool>
{
    public async Task<bool> Handle(DeleteOrganizationUnitCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.OuId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
