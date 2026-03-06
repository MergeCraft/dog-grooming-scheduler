using AplicationLogic.DTOs.Reserve;
using BusinessLogic.Entities;
using System;

namespace AplicationLogic.DTOs.ReserveMapper
{
    public static class CreateReserveMapper
    {
        /// <summary>
        /// Transforma un DTO de creación en una entidad de negocio Reserve.
        /// </summary>
        public static BusinessLogic.Entities.Reserve FromDto(CreateReserveDto dto)
        {
            if (dto == null) return null;

            return new BusinessLogic.Entities.Reserve
            {
                Id = Guid.NewGuid(), // Generamos el nuevo ID único para la reserva
                ReservationDate = dto.ReservationDate,
                TimeSlot = dto.TimeSlot,
                ScheduleId = dto.ScheduleId,
                ClientId = dto.ClientId,
                PetSize = dto.PetSize,
                IsCanceled = false // Por defecto una reserva nueva no está cancelada
            };
        }

        /// <summary>
        /// (Opcional) Útil si necesitas devolver la reserva creada al frontend
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
