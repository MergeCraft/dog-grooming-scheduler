using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.Entities
{
    public class Client : User
    {
        // Telefono
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }

        public List<Reserve> Reservations { get; set; } = new List<Reserve>();
    }
}
