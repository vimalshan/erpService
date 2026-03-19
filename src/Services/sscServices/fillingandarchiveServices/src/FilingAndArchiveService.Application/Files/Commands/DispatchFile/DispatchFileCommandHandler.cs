using FilingAndArchiveService.Application.Common.Interfaces;
using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Domain.Interfaces;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Commands.DispatchFile;

public class DispatchFileCommandHandler : IRequestHandler<DispatchFileCommand, FileMasterDto>
{
    private readonly IFileRepository _fileRepository;
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public DispatchFileCommandHandler(
        IFileRepository fileRepository,
        IApplicationDbContext context,
        IPublisher publisher)
    {
        _fileRepository = fileRepository;
        _context = context;
        _publisher = publisher;
    }

    public async Task<FileMasterDto> Handle(DispatchFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetByIdAsync(request.FileId, cancellationToken)
            ?? throw new Domain.Exceptions.FileNotFoundException(request.FileId);

        file.Dispatch(request.PodNo, request.CourierName, request.DispatchedBy);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in file.DomainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);
        file.ClearDomainEvents();

        return file.ToDto();
    }
}
