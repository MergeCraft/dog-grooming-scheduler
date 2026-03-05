using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.Entities
{
    public class PetGroomer
    {
        [Required]
        public string Name { get; set; }

        public string LastName { get; set; }


        public Schedule Schedule { get; set; } = new Schedule();
    }
}
