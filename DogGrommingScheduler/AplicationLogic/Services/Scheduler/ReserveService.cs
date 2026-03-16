using Shared.DTOs;
using Shared.DTOs.ReserveMapper;
using AplicationLogic.Interfaces;
using BusinessLogic.Results;
using BusinessLogic.Entities;
using Hangfire;
using System;
using System.Threading.Tasks;
using BusinessLogic.RepositoryInterfaces;

namespace AplicationLogic.Services.Scheduler
{
    public class ReserveService : IReserveService
    {
        private readonly IReserveRepository _reserveRepository;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IEmailService _emailService;

        public ReserveService(IReserveRepository repo, IBackgroundJobClient jobs, IEmailService email, IBackgroundJobService backgroundJobService)
        {
            _reserveRepository = repo;
            _backgroundJobs = jobs;
            _emailService = email;
            _backgroundJobService = backgroundJobService;
        }

        public async Task<Result> ProcessNewReserveAsync(CreateReserveDto dto)
        {
            if (dto == null) return Result.Failure(new[] { new Error("Error.InvalidInput", "DTO is null") });

            // Map shared DTO model to domain Reserve entity
            var dtoModel = CreateReserveMapper.FromDto(dto);

            var domainReserve = new Reserve
            {
                Id = dtoModel.Id == Guid.Empty ? Guid.NewGuid() : dtoModel.Id,
                ReservationDate = dtoModel.ReservationDate.Date,
                TimeSlot = dtoModel.TimeSlot,
                ScheduleId = dtoModel.ScheduleId,
                ClientId = dtoModel.ClientId,
                PetSize = (BusinessLogic.Entities.DogSize)dtoModel.PetSize,
                IsCanceled = dtoModel.IsCanceled
            };

            var saveResult = await _reserveRepository.AddAsync(domainReserve);
            if (saveResult.IsFailure) return saveResult;

            var appointmentDateTime = domainReserve.ReservationDate.Date + domainReserve.TimeSlot;

            // Use background job service wrapper to allow mocking in tests
            _backgroundJobService.Enqueue(() =>
                _emailService.SendAppointmentConfirmationAsync(
                    dto.ClientEmail,
                    dto.ClientName,
                    appointmentDateTime));

            DateTime reminderTime = appointmentDateTime.AddHours(-2);
            if (reminderTime > DateTime.Now)
            {
                string jobId = _backgroundJobService.Schedule(() =>
                    _emailService.SendReminderAppointmentAsync(
                        dto.ClientEmail,
                        dto.ClientName,
                        appointmentDateTime,
                        dto.GroomerName),
                    reminderTime);

                domainReserve.ReminderJobId = jobId;
                await _reserveRepository.UpdateAsync(domainReserve);
            }

            if (reminderTime > DateTime.Now)
            {
                string jobId = _backgroundJobs.Schedule(() =>
                    _emailService.SendReminderAppointmentAsync(
                        dto.ClientEmail,
                        dto.ClientName,
                        appointmentDateTime,
                        dto.GroomerName),
                    reminderTime);

                domainReserve.ReminderJobId = jobId;
                await _reserveRepository.UpdateAsync(domainReserve);
            }

            return Result.Success();
        }

        public async Task<Result> CancelReserveAsync(Guid reserveId)
        {
            var result = await _reserveRepository.GetByIdAsync(reserveId);
            if (result.IsFailure) return Result.Failure(result.Errors);

            var reserve = result.Value;

            if (!string.IsNullOrEmpty(reserve.ReminderJobId))
            {
                _backgroundJobService.Delete(reserve.ReminderJobId);
            }

            reserve.IsCanceled = true;
            return await _reserveRepository.UpdateAsync(reserve);
        }
    }
}
