using FilingAndArchiveService.Application.Common.Interfaces;
using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Domain.Entities;
using FilingAndArchiveService.Domain.Interfaces;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Commands.CreateFile;

public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, FileMasterDto>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFilingCounterRepository _counterRepository;
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public CreateFileCommandHandler(
        IFileRepository fileRepository,
        IFilingCounterRepository counterRepository,
        IApplicationDbContext context,
        IPublisher publisher)
    {
        _fileRepository = fileRepository;
        _counterRepository = counterRepository;
        _context = context;
        _publisher = publisher;
    }

    public async Task<FileMasterDto> Handle(CreateFileCommand request, CancellationToken cancellationToken)
    {
        var fileId = await _counterRepository.GetNextCountAsync(request.FileOrgId, cancellationToken);

        var file = FileMaster.Create(
            fileId,
            request.FileOrgId,
            request.FileYear,
            request.FileNo,
            request.CreatedBy,
            request.Remarks);

        await _fileRepository.AddAsync(file, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in file.DomainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);

        file.ClearDomainEvents();

        return file.ToDto();
    }
}
