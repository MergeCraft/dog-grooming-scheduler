using System;
using System.Collections.Generic;
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

        // FechaReserva
        public DateTime ReservationDate { get; set; }

        // Horario (time of the reservation)
        public TimeSpan TimeSlot { get; set; }

        // Peluquero
        public PetGroomer Groomer { get; set; }

        // Cliente
        public Client Client { get; set; }

        // TipoDePerro (small, medium or large)
        public DogSize PetSize { get; set; }
    }
}
