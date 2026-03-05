using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Entities
{
    public class Schedule
    {
        public Guid Id { get; set; }

        public List<Reserve> Reservations { get; set; } = new List<Reserve>();

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
