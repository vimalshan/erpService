namespace BatchAndEnvelopeService.Domain.Exceptions;

public class EnvelopeDomainException : Exception
{
    public EnvelopeDomainException(string message) : base(message) { }
    public EnvelopeDomainException(string message, Exception inner) : base(message, inner) { }
}

public class EnvelopeNotFoundException : EnvelopeDomainException
{
    public EnvelopeNotFoundException(long envelopeId) : base($"Envelope with ID {envelopeId} was not found.") { }
}
