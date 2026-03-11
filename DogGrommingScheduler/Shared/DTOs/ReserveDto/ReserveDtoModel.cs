using System;

namespace Shared.DTOs
{
    public class ReserveDtoModel
    {
        public Guid Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan TimeSlot { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid ClientId { get; set; }
        public DogSize PetSize { get; set; }
        public bool IsCanceled { get; set; }
    }
}
