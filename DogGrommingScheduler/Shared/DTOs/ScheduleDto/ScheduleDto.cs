using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ScheduleDto
    {
        public Guid Id { get; set; }
        public Guid PetGroomerId { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<TimeSpan> OccupiedSlots { get; set; } = new();
    }
}
