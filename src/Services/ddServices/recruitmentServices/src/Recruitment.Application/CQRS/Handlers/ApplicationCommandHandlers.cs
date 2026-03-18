using AutoMapper;
using MediatR;
using Recruitment.Application.CQRS.Commands;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Repositories;
using Recruitment.Domain.ValueObjects;

namespace Recruitment.Application.CQRS.Handlers;

/// <summary>
/// Handler for CreateApplicationCommand
/// </summary>
public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, decimal>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateApplicationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<decimal> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var contactInfo = new ContactInfo(request.ApplicationData.SparshId, request.ApplicationData.SparshPin);
        
        var application = new Domain.Entities.Application(
            request.ApplicationData.ApplicationNumber,
            request.ApplicationData.JobId,
            contactInfo);

        await _unitOfWork.Applications.AddAsync(application);
        await _unitOfWork.SaveChangesAsync();

        return application.ApplicationNumber;
    }
}

/// <summary>
/// Handler for UpdateApplicationCommand
/// </summary>
public class UpdateApplicationCommandHandler : IRequestHandler<UpdateApplicationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateApplicationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _unitOfWork.Applications.GetByIdAsync(request.ApplicationData.ApplicationNumber);
        if (application == null)
            return false;

        application.UpdateApplicationDetails(
            request.ApplicationData.CurrentJobDescription,
            request.ApplicationData.Achievements,
            request.ApplicationData.ReasonForJoining,
            request.ApplicationData.Strength,
            request.ApplicationData.Awards);

        await _unitOfWork.Applications.UpdateAsync(application);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

/// <summary>
/// Handler for ChangeApplicationStatusCommand
/// </summary>
public class ChangeApplicationStatusCommandHandler : IRequestHandler<ChangeApplicationStatusCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeApplicationStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangeApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var application = await _unitOfWork.Applications.GetByIdAsync(request.ApplicationNumber);
        if (application == null)
            return false;

        if (!Enum.TryParse<ApplicationStatus>(request.Status, true, out var status))
            return false;

        application.ChangeStatus(status, request.Remark, request.UpdatedBy);
        await _unitOfWork.Applications.UpdateAsync(application);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

/// <summary>
/// Handler for SetApplicationMarksCommand
/// </summary>
public class SetApplicationMarksCommandHandler : IRequestHandler<SetApplicationMarksCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetApplicationMarksCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SetApplicationMarksCommand request, CancellationToken cancellationToken)
    {
        var application = await _unitOfWork.Applications.GetByIdAsync(request.ApplicationNumber);
        if (application == null)
            return false;

        application.SetMarks(request.CrtMarks, request.DomainMarks);
        await _unitOfWork.Applications.UpdateAsync(application);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

/// <summary>
/// Handler for SetApplicationDocumentsCommand
/// </summary>
public class SetApplicationDocumentsCommandHandler : IRequestHandler<SetApplicationDocumentsCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetApplicationDocumentsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SetApplicationDocumentsCommand request, CancellationToken cancellationToken)
    {
        var application = await _unitOfWork.Applications.GetByIdAsync(request.ApplicationNumber);
        if (application == null)
            return false;

        application.SetDocuments(request.CrtDocumentPath, request.DomainDocumentPath);
        await _unitOfWork.Applications.UpdateAsync(application);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
