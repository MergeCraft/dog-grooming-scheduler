using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.Entities
{
    public enum DogSize
    {
        Small,
        Medium,
        Large
    }

    public class Reserve
    {
        public Guid Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan TimeSlot { get; set; } // The exact start time (e.g., 10:30 AM)

        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public DogSize PetSize { get; set; }
        public bool IsCanceled { get; set; } = false;
        public string ReminderJobId { get; set; }
    }
}
