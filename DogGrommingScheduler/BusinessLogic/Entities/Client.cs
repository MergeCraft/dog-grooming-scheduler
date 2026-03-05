using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }

        public List<Reserve> Reservations { get; set; } = new List<Reserve>();
    }
}
