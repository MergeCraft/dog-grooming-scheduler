using BusinessLogic.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace AplicationLogic.DTOs.Reserve
{
    public class CreateReserveDto
    {
        [Required(ErrorMessage = "La fecha de reserva es obligatoria.")]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        public TimeSpan TimeSlot { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un peluquero.")]
        public Guid PetGroomerId { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public Guid ClientId { get; set; }

        [Required(ErrorMessage = "Debe especificar el tamaño de la mascota.")]
        public DogSize PetSize { get; set; }

        // Opcional: El ID del horario se puede buscar automáticamente en el servicio 
        // usando la fecha y el GroomerId, pero tenerlo aquí facilita las cosas.
        public Guid ScheduleId { get; set; }
        public string ClientEmail { get; set; }
        public string ClientName { get; set; }
        public string GroomerName { get; set; }
    }
}