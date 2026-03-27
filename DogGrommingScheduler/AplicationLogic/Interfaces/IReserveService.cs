using BusinessLogic.Entities;
using BusinessLogic.Results;
using Shared.DTOs;
using Shared.DTOs.ReserveDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationLogic.Interfaces
{
    public interface IReserveService
    {
        Task<Result> ProcessNewReserveAsync(CreateReserveDto dto);
        Task<Result> CancelReserveAsync(Guid reserveId);
        Task<Result<ScheduleDto>> GetScheduleForReservationAsync(Guid groomerId, DateTime date);
        Task<Result<IEnumerable<MyReserveDto>>> GetUserReservesAsync(Guid userId);
    }
}
