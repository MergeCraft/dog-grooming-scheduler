using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Entities
{
    public class Client : User
    {
        // Telefono
        public string Phone { get; set; }

        // Correo
        public string Email { get; set; }

        // ListaReservas
        public List<Reserve> Reservations { get; set; } = new List<Reserve>();
    }
}
