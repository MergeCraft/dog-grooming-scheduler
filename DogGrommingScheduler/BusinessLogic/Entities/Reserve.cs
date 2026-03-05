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

        public TimeSpan TimeSlot { get; set; }
        [Required]
        public PetGroomer Groomer { get; set; }
        [Required]
        public Client Client { get; set; }

        // (small, medium or large)
        public DogSize PetSize { get; set; }

        // Save the ID that Hangfire returns when scheduling the reminder job, so we can cancel it if needed
        public string? ReminderJobId { get; set; }

        public bool IsCanceled { get; set; } = false;
    }
}
