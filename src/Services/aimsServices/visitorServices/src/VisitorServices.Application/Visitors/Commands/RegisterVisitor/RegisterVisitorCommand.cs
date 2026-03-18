using MediatR;
using VisitorServices.Application.DTOs;

namespace VisitorServices.Application.Visitors.Commands.RegisterVisitor;

public sealed record RegisterVisitorCommand(
    string VisitorName,
    char IdType,
    string? IdNumber,
    string? PhoneNumber,
    string? Email,
    string? Company,
    string? Purpose,
    long WhomToVisit,
    long EnteredBy) : IRequest<VisitorDto>;
