using Shared.DTOs;
using BusinessLogic.Entities;
using BusinessLogic.Results;
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
    }
}
