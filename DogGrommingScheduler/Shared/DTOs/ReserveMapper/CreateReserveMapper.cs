using BusinessLogic.Entities;

namespace Shared.DTOs.ReserveMapper
{
    public static class CreateReserveMapper
    {
        public static Reserve ToEntity(CreateReserveDto dto)
        {
            if (dto == null) return null;

            return new Reserve
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                ReservationDate = dto.ReservationDate.Date,
                TimeSlot = dto.TimeSlot,
                ScheduleId = dto.ScheduleId,
                ClientId = dto.ClientId,
                PetSize = (BusinessLogic.Entities.DogSize)dto.PetSize,
                IsCanceled = dto.IsCanceled,

                // CORRECCIÓN: Evita el error Msg 515 al no enviar NULL a una columna requerida
                ReminderJobId = string.Empty
            };
        }

        public static CreateReserveDto FromEntity(Reserve entity)
        {
            if (entity == null) return null;

            return new CreateReserveDto
            {
                Id = entity.Id,
                ReservationDate = entity.ReservationDate,
                TimeSlot = entity.TimeSlot,
                ScheduleId = entity.ScheduleId,
                ClientId = entity.ClientId,
                PetSize = (Shared.DTOs.DogSize)entity.PetSize,
                IsCanceled = entity.IsCanceled
            };
        }
    }
}
