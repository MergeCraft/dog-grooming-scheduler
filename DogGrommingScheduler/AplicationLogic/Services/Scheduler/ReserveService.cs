using AplicationLogic.DTOs.Reserve;
using AplicationLogic.DTOs.ReserveMapper;
using AplicationLogic.Interfaces;
using BusinessLogic.RepositoriesInterfaces;
using BusinessLogic.Results;
using Hangfire;

namespace AplicationLogic.Services.Scheduler
{
    public class ReserveService : IReserveService
    {
        private readonly IReserveRepository _reserveRepository;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IEmailService _emailService;

        public ReserveService(IReserveRepository repo, IBackgroundJobClient jobs, IEmailService email)
        {
            _reserveRepository = repo;
            _backgroundJobs = jobs;
            _emailService = email;
        }

        public async Task<Result> ProcessNewReserveAsync(CreateReserveDto dto)
        {
            var reserve = CreateReserveMapper.FromDto(dto);

            var saveResult = await _reserveRepository.AddAsync(reserve);
            if (saveResult.IsFailure) return saveResult;

            _backgroundJobs.Enqueue(() =>
                _emailService.SendAppointmentConfirmationAsync(
                    dto.ClientEmail,
                    dto.ClientName,
                    reserve.ReservationDate));

            DateTime reminderTime = reserve.ReservationDate.Date + reserve.TimeSlot - TimeSpan.FromHours(2);

            if (reminderTime > DateTime.Now)
            {
                string jobId = _backgroundJobs.Schedule(() =>
                    _emailService.SendReminderAppointmentAsync(
                        dto.ClientEmail,
                        dto.ClientName,
                        reserve.ReservationDate,
                        dto.GroomerName),
                    reminderTime);

                reserve.ReminderJobId = jobId;
                await _reserveRepository.UpdateAsync(reserve);
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
    }
}
