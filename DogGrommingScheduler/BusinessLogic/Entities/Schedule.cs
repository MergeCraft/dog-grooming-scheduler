using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid PetGroomerId { get; set; }
    public PetGroomer Groomer { get; set; } = null!;
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<Reserve> Reservations { get; set; } = new List<Reserve>();
}
