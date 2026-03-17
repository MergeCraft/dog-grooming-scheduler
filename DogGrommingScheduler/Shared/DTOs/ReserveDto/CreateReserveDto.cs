using System;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.Entities;

namespace Shared.DTOs
{
    public class CreateReserveDto
    {
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "El horario es obligatorio.")]
        public TimeSpan TimeSlot { get; set; }

        [Required]
        public Guid PetGroomerId { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public DogSize PetSize { get; set; }

        public Guid ScheduleId { get; set; }

        public string? ClientEmail { get; set; }
        public string? ClientName { get; set; }
        public string? GroomerName { get; set; }
    }
}
