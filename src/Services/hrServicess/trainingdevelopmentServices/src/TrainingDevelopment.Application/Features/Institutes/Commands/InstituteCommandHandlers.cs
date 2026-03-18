using AutoMapper;
using MediatR;
using TrainingDevelopment.Application.Common.Exceptions;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Domain.Entities;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.Institutes.Commands;

public class CreateInstituteCommandHandler : IRequestHandler<CreateInstituteCommand, InstituteMasterDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateInstituteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<InstituteMasterDto> Handle(CreateInstituteCommand request, CancellationToken cancellationToken)
    {
        var entity = InstituteMaster.Create(
            request.InstituteCode, request.InstituteName,
            request.Address1, request.Address2,
            request.City, request.State, request.Pin,
            request.Phone, request.Fax, request.Email,
            request.Url, request.InstituteType,
            request.CampusRecruit, request.InstituteClass, request.ModifiedBy);

        await _unitOfWork.Institutes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<InstituteMasterDto>(entity);
    }
}

public class DeleteInstituteCommandHandler : IRequestHandler<DeleteInstituteCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteInstituteCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteInstituteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Institutes.GetByCodeAsync(request.Code, cancellationToken)
            ?? throw new NotFoundException(nameof(InstituteMaster), request.Code);
        _unitOfWork.Institutes.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
