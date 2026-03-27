using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.ReserveDto
{
    public class MyReserveDto
    {
        public Guid Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan TimeSlot { get; set; }
        public string PetSize { get; set; } = string.Empty;
        public bool IsCanceled { get; set; }
    }
}
