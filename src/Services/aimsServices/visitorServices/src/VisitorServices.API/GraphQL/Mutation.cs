using MediatR;
using VisitorServices.Application.Approvals.Commands.ProcessApproval;
using VisitorServices.Application.DTOs;
using VisitorServices.Application.Visitors.Commands.CheckoutVisitor;
using VisitorServices.Application.Visitors.Commands.RegisterVisitor;

namespace VisitorServices.API.GraphQL;

public class Mutation
{
    [GraphQLDescription("Register a new visitor.")]
    public async Task<VisitorDto> RegisterVisitor(
        [Service] ISender sender,
        RegisterVisitorInput input,
        CancellationToken cancellationToken)
    {
        var command = new RegisterVisitorCommand(
            input.VisitorName, input.IdType[0], input.IdNumber,
            input.PhoneNumber, input.Email, input.Company,
            input.Purpose, input.WhomToVisit, input.EnteredBy);

        return await sender.Send(command, cancellationToken);
    }

    [GraphQLDescription("Check out a visitor.")]
    public async Task<bool> CheckoutVisitor(
        [Service] ISender sender,
        long visitorId,
        long checkedOutBy,
        CancellationToken cancellationToken)
    {
        await sender.Send(new CheckoutVisitorCommand(visitorId, checkedOutBy), cancellationToken);
        return true;
    }

    [GraphQLDescription("Approve or reject an approval request.")]
    public async Task<ApprovalRequestDto> ProcessApproval(
        [Service] ISender sender,
        long requestId,
        bool isApproved,
        string? remarks,
        long processedBy,
        CancellationToken cancellationToken)
        => await sender.Send(
            new ProcessApprovalCommand(requestId, isApproved, remarks, processedBy),
            cancellationToken);
}

public sealed record RegisterVisitorInput(
    string VisitorName,
    string IdType,
    string? IdNumber,
    string? PhoneNumber,
    string? Email,
    string? Company,
    string? Purpose,
    long WhomToVisit,
    long EnteredBy);
