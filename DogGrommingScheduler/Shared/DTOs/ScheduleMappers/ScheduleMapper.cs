using BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.ScheduleMappers
{
    public static class ScheduleMapper
    {
        public static ScheduleDto ToDto(Schedule entity)
        {
            return new ScheduleDto
            {
                Id = entity.Id,
                PetGroomerId = entity.PetGroomerId,
                Date = entity.Date,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                OccupiedSlots = entity.Reservations
                    .Where(r => !r.IsCanceled)
                    .Select(r => r.TimeSlot)
                    .ToList()
            };
        }
    }
}
