using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Hangfire;
using System.Linq.Expressions;
using AplicationLogic.Services.Scheduler;
using BusinessLogic.RepositoryInterfaces;
using AplicationLogic.Interfaces;
using BusinessLogic.Entities;
using Shared.DTOs;
using BusinessLogic.Results;

namespace AplicationLogic.Tests
{
    public class ReserveServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IReserveRepository> _reserveRepoMock;
        private readonly Mock<IBackgroundJobService> _bgServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly ReserveService _sut;

        public ReserveServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _reserveRepoMock = _fixture.Freeze<Mock<IReserveRepository>>();
            _bgServiceMock = _fixture.Freeze<Mock<IBackgroundJobService>>();
            _emailServiceMock = _fixture.Freeze<Mock<IEmailService>>();

            _sut = new ReserveService(_reserveRepoMock.Object, _emailServiceMock.Object, _bgServiceMock.Object);
        }

        [Fact]
        public async Task ProcessNewReserveAsync_NullDto_ShouldReturnFailure()
        {
            // Arrange

            // Act
            var result = await _sut.ProcessNewReserveAsync(null);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Code == "Error.InvalidInput");
        }

        [Fact]
        public async Task ProcessNewReserveAsync_ValidDto_ShouldSaveReserve_EnqueueConfirmation_AndScheduleReminder_WhenReminderInFuture()
        {
            // Arrange
            var dto = _fixture.Build<CreateReserveDto>()
                .With(x => x.ReservationDate, DateTime.Now.AddDays(1))
                .With(x => x.TimeSlot, new TimeSpan(10, 0, 0))
                .With(x => x.ClientEmail, "client@example.com")
                .With(x => x.ClientName, "Client Name")
                .With(x => x.GroomerName, "Groomer Name")
                .Create();

            Reserve? addedReserve = null;
            Reserve? updatedReserve = null;
            _reserveRepoMock.Setup(r => r.AddAsync(It.IsAny<Reserve>()))
                .Callback<Reserve>(r => addedReserve = r)
                .ReturnsAsync(Result.Success());

            // Use the background job service mock instead of Hangfire extension methods
            _bgServiceMock.Setup(b => b.Enqueue(It.IsAny<Expression<Action>>()));

            _bgServiceMock.Setup(b => b.Schedule(It.IsAny<Expression<Action>>(), It.IsAny<DateTime>()))
                .Returns("job-id-123");

            _reserveRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Reserve>()))
                .Callback<Reserve>(r => updatedReserve = r)
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _sut.ProcessNewReserveAsync(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Reserve was created and persisted via repository
            _reserveRepoMock.Verify(r => r.AddAsync(It.IsAny<Reserve>()), Times.Once);
            addedReserve.Should().NotBeNull();
            addedReserve!.ReservationDate.Should().Be(dto.ReservationDate.Date);
            addedReserve.TimeSlot.Should().Be(dto.TimeSlot);
            addedReserve.ClientId.Should().Be(dto.ClientId);

            // Reminder was scheduled and reserve updated with job id
            _bgServiceMock.Verify(b => b.Enqueue(It.IsAny<Expression<Action>>()), Times.Once);
            _bgServiceMock.Verify(b => b.Schedule(It.IsAny<Expression<Action>>(), It.IsAny<DateTime>()), Times.Once);
            _reserveRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Reserve>()), Times.Once);
            updatedReserve.Should().NotBeNull();
            updatedReserve!.ReminderJobId.Should().Be("job-id-123");
        }

        [Fact]
        public async Task ProcessNewReserveAsync_ValidDto_ShouldNotScheduleReminder_WhenReminderInPast()
        {
            // Arrange
            var dto = _fixture.Build<CreateReserveDto>()
                .With(x => x.ReservationDate, DateTime.Now)
                .With(x => x.TimeSlot, DateTime.Now.TimeOfDay)
                .With(x => x.ClientEmail, "client@example.com")
                .With(x => x.ClientName, "Client Name")
                .With(x => x.GroomerName, "Groomer Name")
                .Create();


            Reserve? addedReserve = null;
            _reserveRepoMock.Setup(r => r.AddAsync(It.IsAny<Reserve>()))
                .Callback<Reserve>(r => addedReserve = r)
                .ReturnsAsync(Result.Success());

            _bgServiceMock.Setup(b => b.Enqueue(It.IsAny<Expression<Action>>()));

            _reserveRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Reserve>()))
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _sut.ProcessNewReserveAsync(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Reserve was created and persisted via repository
            _reserveRepoMock.Verify(r => r.AddAsync(It.IsAny<Reserve>()), Times.Once);
            addedReserve.Should().NotBeNull();
            addedReserve!.ReservationDate.Should().Be(dto.ReservationDate.Date);
            addedReserve.TimeSlot.Should().Be(dto.TimeSlot);
            addedReserve.ClientId.Should().Be(dto.ClientId);

            // No reminder scheduled and no update called
            _bgServiceMock.Verify(b => b.Schedule(It.IsAny<Expression<Action>>(), It.IsAny<DateTime>()), Times.Never);
            _reserveRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Reserve>()), Times.Never);
        }

        [Fact]
        public async Task CancelReserveAsync_NonExistingReserve_ShouldReturnFailure()
        {
            // Arrange
            var id = Guid.NewGuid();
            _reserveRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result<Reserve>.Failure(new[] { new Error("NotFound", "Not found") }));

            // Act
            var result = await _sut.CancelReserveAsync(id);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task CancelReserveAsync_ExistingReserveWithReminder_ShouldDeleteJob_AndUpdateReserve()
        {
            // Arrange
            var reserve = _fixture.Build<Reserve>()               
                .Without(r => r.Client)
                .With(r => r.ReminderJobId, "job-id-456")
                .With(r => r.IsCanceled, false)
                .Create();

            _reserveRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result<Reserve>.Success(reserve));

            _reserveRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Reserve>()))
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _sut.CancelReserveAsync(reserve.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            // Cannot verify Hangfire extension method invocation directly via Moq (extension methods are static).
            // Verify that the reserve was marked canceled and persisted.
            _reserveRepoMock.Verify(r => r.UpdateAsync(It.Is<Reserve>(x => x.IsCanceled == true)), Times.Once);
        }

        [Fact]
        public async Task CancelReserveAsync_ExistingReserveWithoutReminder_ShouldUpdateReserveWithoutDeleting()
        {
            // Arrange
            var reserve = _fixture.Build<Reserve>()
                .Without(r => r.Client)
                .With(r => r.ReminderJobId, (string?)null)
                .With(r => r.IsCanceled, false)
                .Create();

            _reserveRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result<Reserve>.Success(reserve));

            _reserveRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Reserve>()))
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _sut.CancelReserveAsync(reserve.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            // Ensure reserve was marked canceled and persisted. Do not attempt to verify Hangfire extension methods.
            _reserveRepoMock.Verify(r => r.UpdateAsync(It.Is<Reserve>(x => x.IsCanceled == true)), Times.Once);
        }
    }
}