using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Entities;
public class PetGroomer
{
    public class PetGroomer : User
    {

    public List<Schedule> Schedules { get; set; } = new List<Schedule>();
}
