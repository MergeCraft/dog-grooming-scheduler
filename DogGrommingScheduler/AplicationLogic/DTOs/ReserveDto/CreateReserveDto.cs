using BusinessLogic.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace AplicationLogic.DTOs.Reserve
{
    public class CreateReserveDto
    {
        [Required(ErrorMessage = "Reservation date is required.")]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan TimeSlot { get; set; }

        [Required(ErrorMessage = "A groomer must be selected.")]
        public Guid PetGroomerId { get; set; }

        [Required(ErrorMessage = "Client is required.")]
        public Guid ClientId { get; set; }

        [Required(ErrorMessage = "Pet size must be specified.")]
        public DogSize PetSize { get; set; }

        // Optional: The schedule ID can be looked up automatically in the service
        // using the date and GroomerId, but including it here simplifies things.
        public Guid ScheduleId { get; set; }
        public string ClientEmail { get; set; }
        public string ClientName { get; set; }
        public string GroomerName { get; set; }
    }
}