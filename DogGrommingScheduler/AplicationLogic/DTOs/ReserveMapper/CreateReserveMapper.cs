using AplicationLogic.DTOs.Reserve;
using BusinessLogic.Entities;
using System;

namespace AplicationLogic.DTOs.ReserveMapper
{
    public static class CreateReserveMapper
    {
        /// <summary>
        /// Transforms a creation DTO into a business Reserve entity.
        /// </summary>
        public static BusinessLogic.Entities.Reserve FromDto(CreateReserveDto dto)
        {
            if (dto == null) return null;

            return new BusinessLogic.Entities.Reserve
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
        public static CreateReserveDto ToDto(BusinessLogic.Entities.Reserve entity)
        {
            if (entity == null) return null;

            return new CreateReserveDto
            {
                ReservationDate = entity.ReservationDate,
                TimeSlot = entity.TimeSlot,
                PetGroomerId = entity.Schedule?.PetGroomerId ?? Guid.Empty,
                ClientId = entity.ClientId,
                PetSize = entity.PetSize,
                ScheduleId = entity.ScheduleId
            };
        }
    }
}
