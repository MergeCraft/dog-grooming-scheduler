using System;
using Shared.DTOs;

namespace Shared.DTOs.ReserveMapper
{
    public static class CreateReserveMapper
    {
        /// <summary>
        /// Transforms a creation DTO into a shared Reserve model.
        /// </summary>
        public static ReserveDtoModel FromDto(CreateReserveDto dto)
        {
            if (dto == null) return null;

            return new ReserveDtoModel
            {
                Id = Guid.NewGuid(), // Generate a new unique ID for the reserve
                ReservationDate = dto.ReservationDate,
                TimeSlot = dto.TimeSlot,
                ScheduleId = dto.ScheduleId,
                ClientId = dto.ClientId,
                PetSize = dto.PetSize,
                IsCanceled = false // By default a new reserve is not canceled
            };
        }

        /// <summary>
        /// (Optional) Useful if you need to return the created reserve to the frontend
        /// </summary>
        public static CreateReserveDto ToDto(ReserveDtoModel entity)
        {
            if (entity == null) return null;

            return new CreateReserveDto
            {
                ReservationDate = entity.ReservationDate,
                TimeSlot = entity.TimeSlot,
                PetGroomerId = entity.ScheduleId == Guid.Empty ? Guid.Empty : entity.ScheduleId,
                ClientId = entity.ClientId,
                PetSize = entity.PetSize,
                ScheduleId = entity.ScheduleId
            };
        }
    }
}
