using AutoMapper;
using CSA.Service.Application.DTOs;
using CSA.Service.Domain.Entities;
using CSA.Service.Domain.Interfaces;
using MediatR;

namespace CSA.Service.Application.Commands.Processes;

public class CreateProcessCommandHandler(
    IProcessRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<CreateProcessCommand, ProcessDto>
{
    public async Task<ProcessDto> Handle(CreateProcessCommand request, CancellationToken ct)
    {
        var process = mapper.Map<Process>(request.Dto);
        process.CreatedBy = request.UserId;
        process.CreatedOn = DateTime.UtcNow;

        var created = await repository.AddAsync(process, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<ProcessDto>(created);
    }
}

public class CreateSubProcessCommandHandler(
    ISubProcessRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<CreateSubProcessCommand, SubProcessDto>
{
    public async Task<SubProcessDto> Handle(CreateSubProcessCommand request, CancellationToken ct)
    {
        var subProcess = mapper.Map<SubProcess>(request.Dto);
        subProcess.CreatedBy = request.UserId;
        subProcess.CreatedOn = DateTime.UtcNow;

        var created = await repository.AddAsync(subProcess, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<SubProcessDto>(created);
    }
}

public class DeleteProcessCommandHandler(
    IProcessRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProcessCommand, bool>
{
    public async Task<bool> Handle(DeleteProcessCommand request, CancellationToken ct)
    {
        await repository.DeleteAsync(request.ProcessId, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
