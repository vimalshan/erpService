using MediatR;
using Todos.Application.DTOs;
using Todos.Application.Queries;

namespace Todos.API.GraphQL.Learning;

/// <summary>
/// GraphQL Query type for Learning module
/// </summary>
public class LearningQuery
{
    private readonly IMediator _mediator;

    public LearningQuery(IMediator mediator)
    {
        _mediator = mediator;
    }

    [GraphQLName("getLearningRecord")]
    public async Task<LearningRecordDto?> GetLearningRecord(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLearningRecordByIdQuery { Id = id }, cancellationToken);
        return result.Data;
    }

    [GraphQLName("getAllLearningRecords")]
    public async Task<IEnumerable<LearningRecordDto>> GetAllLearningRecords(int pageNumber = 1, int pageSize = 10, CancellationToken? cancellationToken = null)
    {
        var ct = cancellationToken ?? CancellationToken.None;
        var result = await _mediator.Send(new GetAllLearningRecordsQuery { PageNumber = pageNumber, PageSize = pageSize }, ct);
        return result.Data ?? [];
    }

    [GraphQLName("searchLearningRecords")]
    public async Task<IEnumerable<LearningRecordDto>> SearchLearningRecords(decimal requestNumber, CancellationToken? cancellationToken = null)
    {
        var ct = cancellationToken ?? CancellationToken.None;
        var result = await _mediator.Send(new SearchLearningRecordsByRequestNumberQuery { RequestNumber = requestNumber }, ct);
        return result.Data ?? [];
    }

    [GraphQLName("getLearningFeedback")]
    public async Task<LearningFeedbackDto?> GetLearningFeedback(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLearningFeedbackByIdQuery { Id = id }, cancellationToken);
        return result.Data;
    }

    [GraphQLName("getAllLearningFeedback")]
    public async Task<IEnumerable<LearningFeedbackDto>> GetAllLearningFeedback(int pageNumber = 1, int pageSize = 10, CancellationToken? cancellationToken = null)
    {
        var ct = cancellationToken ?? CancellationToken.None;
        var result = await _mediator.Send(new GetAllLearningFeedbackQuery { PageNumber = pageNumber, PageSize = pageSize }, ct);
        return result.Data ?? [];
    }
}
