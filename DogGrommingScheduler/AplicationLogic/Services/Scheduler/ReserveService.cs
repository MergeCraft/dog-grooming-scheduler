using AplicationLogic.Interfaces;
using BusinessLogic.Entities;
using BusinessLogic.RepositoriesInterfaces;
using BusinessLogic.RepositoryInterfaces;
using BusinessLogic.Results;
using Hangfire;
using Shared.DTOs;
using Shared.DTOs.ReserveMapper;
using System;
using System.Threading.Tasks;

namespace AplicationLogic.Services.Scheduler
{
    public class ReserveService : IReserveService
    {
        private readonly IReserveRepository _reserveRepository;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IEmailService _emailService;
        private readonly IScheduleRepository _scheduleRepository;

        public ReserveService(IReserveRepository repo, IBackgroundJobClient jobs, IEmailService email,IScheduleRepository scheduleRepository)
        {
            _reserveRepository = repo;
            _backgroundJobs = jobs;
            _emailService = email;
            _scheduleRepository = scheduleRepository;
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

            _backgroundJobs.Enqueue(() =>
                _emailService.SendAppointmentConfirmationAsync(
                    dto.ClientEmail,
                    dto.ClientName,
                    appointmentDateTime));

            DateTime reminderTime = appointmentDateTime.AddHours(-2);

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
                _backgroundJobs.Delete(reserve.ReminderJobId);
            }

            reserve.IsCanceled = true;
            return await _reserveRepository.UpdateAsync(reserve);
        }
        public async Task<Result<ScheduleDto>> GetScheduleForReservationAsync(Guid groomerId, DateTime date)
        {
            var result = await _scheduleRepository.GetByGroomerAndDateAsync(groomerId, date);

            if (result.IsFailure)
                return Result<ScheduleDto>.Failure(result.Errors);

            if (result.Value == null)
            {
                return Result<ScheduleDto>.Failure(new[] {
            new Error("Error.NotFound", "No hay agenda precargada para este peluquero en esa fecha.")
        });
            }

            var dto = Shared.DTOs.ScheduleMappers.ScheduleMapper.ToDto(result.Value);

            return Result<ScheduleDto>.Success(dto);
        }
    }
}
