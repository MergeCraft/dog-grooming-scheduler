using BusinessLogic.Entities;
using Shared.DTOs.ReserveDto;

namespace Shared.DTOs.ReserveMapper
{
    public static class MyReserveMapper
    {
        public static MyReserveDto ToDto(Reserve entity)
        {
            if (entity == null) return null;

            return new MyReserveDto
            {
                Id = entity.Id,
                ReservationDate = entity.ReservationDate,
                TimeSlot = entity.TimeSlot,
                PetSize = entity.PetSize.ToString(),
                IsCanceled = entity.IsCanceled
            };
        }
    }
}
