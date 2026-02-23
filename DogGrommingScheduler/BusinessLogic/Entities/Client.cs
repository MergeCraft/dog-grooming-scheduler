using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Entities
{
    internal class Client
    {
        public Guid Id { get; set; }

        // Nombre
        public string Name { get; set; }

        // Telefono
        public string Phone { get; set; }

        // Correo
        public string Email { get; set; }

        // ListaReservas
        public List<Reserve> Reservations { get; set; } = new List<Reserve>();
    }
}
