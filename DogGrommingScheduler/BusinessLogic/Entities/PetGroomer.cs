using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Entities;
public class PetGroomer
{
    public Guid Id { get; set; } 
    [Required]
    public string Name { get; set; }
    public string LastName { get; set; }

    public List<Schedule> Schedules { get; set; } = new List<Schedule>();
}
