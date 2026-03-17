using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Resend;
using AplicationLogic.Services.Email;

namespace AplicationLogic.Tests
{
    public class ResendEmailServiceTests
    {
        [Fact]
        public async Task SendAppointmentConfirmationAsync_ShouldCallResendWithExpectedMessage()
        {
            // Arrange
            var resendMock = new Mock<IResend>();
            EmailMessage? captured = null;
            resendMock.Setup(r => r.EmailSendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
                .Callback<EmailMessage, CancellationToken>((m, ct) => captured = m)
                .ReturnsAsync((ResendResponse<Guid>?)null);

            var service = new ResendEmailService(resendMock.Object);
            var appointment = new DateTime(2026, 3, 20, 14, 30, 0);
            var to = "client@example.com";
            var clientName = "John Doe";

            // Act
            await service.SendAppointmentConfirmationAsync(to, clientName, appointment);

            // Assert
            resendMock.Verify(r => r.EmailSendAsync(It.IsAny<EmailMessage>()), Times.Once);
            captured.Should().NotBeNull();
            captured!.From.ToString().Should().Be("schedule@updates.riosrenato.com");
            captured.To.Should().Contain(to);
            captured.Subject.Should().Contain("Date Confirmin");
            captured.HtmlBody.Should().Contain($"Hi {clientName}");
            captured.HtmlBody.Should().Contain(appointment.ToString("MMMM dd, yyyy"));
            captured.HtmlBody.Should().Contain(appointment.ToString("hh:mm tt"));
        }

        [Fact]
        public async Task SendReminderAppointmentAsync_ShouldCallResendWithExpectedMessage()
        {
            // Arrange
            var resendMock = new Mock<IResend>();
            EmailMessage? captured = null;
            resendMock.Setup(r => r.EmailSendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
                .Callback<EmailMessage, CancellationToken>((m, ct) => captured = m)
                .ReturnsAsync((ResendResponse<Guid>?)null);

            var service = new ResendEmailService(resendMock.Object);
            var appointment = new DateTime(2026, 4, 5, 9, 15, 0);
            var to = "client2@example.com";
            var clientName = "Jane Roe";
            var groomer = "Alex";

            // Act
            await service.SendReminderAppointmentAsync(to, clientName, appointment, groomer);

            // Assert
            resendMock.Verify(r => r.EmailSendAsync(It.IsAny<EmailMessage>()), Times.Once);
            captured.Should().NotBeNull();
            captured!.From.ToString().Should().Be("schedule@updates.riosrenato.com");
            captured.To.Should().Contain(to);
            captured.Subject.Should().Contain("Reserve reminder");
            captured.HtmlBody.Should().Contain($"Hi {clientName}");
            captured.HtmlBody.Should().Contain(groomer);
            captured.HtmlBody.Should().Contain(appointment.ToString("MMMM dd, yyyy"));
            captured.HtmlBody.Should().Contain(appointment.ToString("hh:mm tt"));
        }
    }
}
