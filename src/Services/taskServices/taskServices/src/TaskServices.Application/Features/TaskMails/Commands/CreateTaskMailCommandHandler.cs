using MediatR;
using TaskServices.Domain.Entities;
using TaskServices.Domain.Repositories;
using TaskServices.Domain.ValueObjects;

namespace TaskServices.Application.Features.TaskMails.Commands;

public class CreateTaskMailCommandHandler : IRequestHandler<CreateTaskMailCommand, decimal>
{
    private readonly ITaskMailRepository _repository;

    public CreateTaskMailCommandHandler(ITaskMailRepository repository)
    {
        _repository = repository;
    }

    public async Task<decimal> Handle(CreateTaskMailCommand request, CancellationToken cancellationToken)
    {
        var mailId = new MailId(request.MID);
        var sysId = new SystemUserId(request.SYSID);
        var taskMail = new TaskMail(mailId, sysId);

        await _repository.AddAsync(taskMail, cancellationToken);
        return taskMail.MID;
    }
}
