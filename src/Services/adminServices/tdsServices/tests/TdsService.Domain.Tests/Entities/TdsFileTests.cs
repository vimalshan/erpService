using FluentAssertions;
using TdsService.Domain.Entities;
using TdsService.Domain.Events;
using TdsService.Domain.Exceptions;
using TdsService.Domain.ValueObjects;
using Xunit;

namespace TdsService.Domain.Tests.Entities;

public sealed class TdsFileTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        var file = TdsFile.Create(101, "Form16A.pdf", "ABCDE1234F", "N", "16A");

        file.Id.Should().Be(101);
        file.FileName.Should().Be("Form16A.pdf");
        file.PanNumber!.Value.Should().Be("ABCDE1234F");
        file.EmailStatus.Should().Be(EmailStatus.Pending);
        file.FileType!.Value.Should().Be("16A");
    }

    [Fact]
    public void Create_ShouldRaiseTdsFileUploadedEvent()
    {
        var file = TdsFile.Create(101, "Form16A.pdf", null, "N", null);

        file.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<TdsFileUploadedEvent>();
    }

    [Fact]
    public void MarkEmailSent_ShouldUpdateStatusAndRaiseEvent()
    {
        var file = TdsFile.Create(101, "Form16A.pdf", "ABCDE1234F", "N", "16A");
        file.ClearDomainEvents();

        file.MarkEmailSent();

        file.EmailStatus.Should().Be(EmailStatus.Sent);
        file.UpdatedAt.Should().NotBeNull();
        file.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<TdsFileEmailSentEvent>();
    }

    [Fact]
    public void MarkEmailSent_WhenAlreadySent_ShouldThrowDomainException()
    {
        var file = TdsFile.Create(101, "Form16A.pdf", null, "Y", null);
        file.ClearDomainEvents();

        var act = () => file.MarkEmailSent();
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void SetBlobUri_WithValidUri_ShouldSucceed()
    {
        var file = TdsFile.Create(101, "Form16A.pdf", null, "N", null);

        file.SetBlobUri("https://storage.example.com/tds-files/Form16A.pdf");

        file.BlobStorageUri.Should().NotBeNullOrEmpty();
    }
}
