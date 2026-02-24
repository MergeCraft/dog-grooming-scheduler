using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Entities
{
    public class PetGroomer : User
    {

        // Apellido
        public string LastName { get; set; }

        // Agenda (contains reservations)
        public Schedule Schedule { get; set; } = new Schedule();
    }
}
