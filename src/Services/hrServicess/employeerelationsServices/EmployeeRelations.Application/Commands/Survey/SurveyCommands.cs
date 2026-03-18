using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Domain.Aggregates;
using EmployeeRelations.Domain.Exceptions;
using FluentValidation;
using AutoMapper;

namespace EmployeeRelations.Application.Commands.Survey;

// ---- Create Survey ----
public record CreateSurveyCommand(string Name, string Image, DateTime StartDate, DateTime? EndDate, string AutoLock, long? TemplateId) : IRequest<SurveyMasterDto>;

public class CreateSurveyValidator : AbstractValidator<CreateSurveyCommand>
{
    public CreateSurveyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Image).NotEmpty().MaximumLength(100);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.AutoLock).NotEmpty().Must(v => v == "Y" || v == "N");
    }
}

public class CreateSurveyHandler : IRequestHandler<CreateSurveyCommand, SurveyMasterDto>
{
    private readonly ISurveyRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateSurveyHandler(ISurveyRepository repo, IUnitOfWork uow, IMapper mapper) { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<SurveyMasterDto> Handle(CreateSurveyCommand req, CancellationToken ct)
    {
        var id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var survey = new SurveyMaster(id, req.Name, req.Image, req.StartDate, req.EndDate, req.AutoLock);
        await _repo.AddAsync(survey, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<SurveyMasterDto>(survey);
    }
}

// ---- Submit Survey Response ----
public record SubmitSurveyResponseCommand(long SurveyId, long EmpSysId, long UpdatedBy, IEnumerable<ResponseDetailRequest> Details) : IRequest<SurveyResponseDto>;
public record ResponseDetailRequest(long QuestionId, string? Option, string? Text);

public class SubmitSurveyResponseValidator : AbstractValidator<SubmitSurveyResponseCommand>
{
    public SubmitSurveyResponseValidator()
    {
        RuleFor(x => x.SurveyId).GreaterThan(0);
        RuleFor(x => x.EmpSysId).GreaterThan(0);
    }
}

public class SubmitSurveyResponseHandler : IRequestHandler<SubmitSurveyResponseCommand, SurveyResponseDto>
{
    private readonly ISurveyRepository _repo;
    private readonly IUnitOfWork _uow;

    public SubmitSurveyResponseHandler(ISurveyRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<SurveyResponseDto> Handle(SubmitSurveyResponseCommand req, CancellationToken ct)
    {
        var survey = await _repo.GetByIdAsync(req.SurveyId, ct)
            ?? throw new EntityNotFoundException(nameof(SurveyMaster), req.SurveyId);
        var responseId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        survey.AddResponse(responseId, req.EmpSysId, req.UpdatedBy);
        var response = survey.Responses.Last();
        foreach (var d in req.Details)
            response.AddDetail(d.QuestionId, d.Option, d.Text);
        response.Submit();
        await _repo.AddResponseAsync(response, ct);
        await _uow.SaveChangesAsync(ct);
        return new SurveyResponseDto(response.ResponseId, response.SurveyId, response.EmpSysId,
            response.Status, response.UpdatedOn,
            response.Details.Select(d => new SurveyResponseDetailDto(d.QuestionId, d.ResponseOption, d.ResponseText)));
    }
}
