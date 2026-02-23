using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Entities
{
    internal class PetGroomer
    {
        // Nombre
        public string Name { get; set; }

        // Apellido
        public string LastName { get; set; }

        // Agenda (contains reservations)
        public Schedule Schedule { get; set; } = new Schedule();
    }
}
